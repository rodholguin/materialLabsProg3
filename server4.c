#include <sys/types.h>
#include <sys/socket.h>
#include <stdio.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <signal.h>
#include <stdlib.h>
#include <pthread.h>

void *thread_func(void *tsocket){
    long c_sockfd = (long)tsocket;
    char ch;
    //LECTURA Y ESCRITURA
    read(c_sockfd, &ch, 1);
    sleep(3);
    ch++;
    write(c_sockfd, &ch, 1);
    close(c_sockfd);
    pthread_exit(NULL);
}

int main(){
    int server_sockfd, client_sockfd;
    int server_len, client_len;
    struct sockaddr_in server_address;
    struct sockaddr_in client_address;

    server_sockfd = socket(AF_INET, SOCK_STREAM, 0); //CREAR SOCKET

    //NOMBRANDO AL SOCKET
    server_address.sin_family = AF_INET;
    server_address.sin_addr.s_addr = htonl(INADDR_ANY);
    server_address.sin_port = htons(9734);
    server_len = sizeof(server_address);
    bind(server_sockfd, (struct sockaddr *)&server_address, server_len); //BIND
    
    listen(server_sockfd, 5); //ESPERANDO A CLIENTES

    while(1){
        pthread_t thread_id;
        printf("server waiting\n");

        client_len = sizeof(client_address);
        client_sockfd = accept(server_sockfd, (struct sockaddr *)&client_address, &client_len);
        
        int ret = pthread_create(&thread_id, NULL, thread_func, (void*)client_sockfd);
        if(ret != 0){printf("Error from pthread %d\n", ret);exit(1);}   
    }
}
