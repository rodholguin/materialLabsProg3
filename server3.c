#include <sys/types.h>
#include <sys/socket.h>
#include <stdio.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <signal.h>
#include <stdlib.h>

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
    signal(SIGCHLD, SIG_IGN);

    while(1){
        char ch;
        printf("server waiting\n");

        client_len = sizeof(client_address);
        client_sockfd = accept(server_sockfd, (struct sockaddr *)&client_address, &client_len);

        if(fork()==0){
            read(client_sockfd, &ch, 1);
            sleep(3);
            ch++;
            write(client_sockfd, &ch, 1);
            close(client_sockfd);
            exit(0);
        } else{
            close(client_sockfd);
        }

    }
}

/* CODIGO PARA UN SOLO CLIENTE

    //CREANDO COLA DE CONEXIONES Y ESPERANDO A LOS CLIENTES
    
    while(1){
        char ch;
        printf("server waiting\n");

        //ACEPTANDO CONEXIÓN
        client_len = sizeof(client_address);
        client_sockfd = accept(server_sockfd, (struct sockaddr *)&client_address, &client_len);

        //LEER Y ESCRIBIR EN EL SOCK DEL CLIENTE
        read(client_sockfd, &ch, 1);
        ch++;
        write(client_sockfd, &ch, 1);
        close(client_sockfd);
    }
 */