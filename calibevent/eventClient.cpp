
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <event2/event.h>
#include <event2/bufferevent.h>
#include <event2/buffer.h>
//#include <netinet/tcp.h>


void read_cb(struct bufferevent* bev, void* ctx)
{
    char buf[1024];
    int n;
    struct evbuffer* input = bufferevent_get_input(bev);
    while ((n = evbuffer_remove(input, buf, sizeof(buf))) > 0) {
        fwrite(buf, 1, n, stdout);
    }
}

void write_cb(struct bufferevent* bev, void* ctx)
{

}

void bufferevent_cb(struct bufferevent* bev, short events, void* ctx)
{
    if (events & BEV_EVENT_CONNECTED) {
        /* We're connected to 127.0.0.1:8080.   Ordinarily we'd do
           something here, like start reading or writing. */
        puts("BEV_EVENT_CONNECTED");
    }
    else if (events & (BEV_EVENT_ERROR | BEV_EVENT_EOF)) {
        /* An error occured while connecting. */
        puts("BEV_EVENT_ERROR");
    }
}



int main2(int argc, char** argv)
{
#ifdef _WIN32
    WSADATA wsa_data;
    WSAStartup(0x0201, &wsa_data);
#endif

    // build the message to be sent
    int length = 800; // the size of message
    char mesg[] = "abasfasjfd";

    // build socket
    int port = 8080;
    struct sockaddr_in my_address;
    memset(&my_address, 0, sizeof(my_address));
    my_address.sin_family = AF_INET;
    my_address.sin_addr.s_addr = htonl(0x7f000001); // 127.0.0.1
    my_address.sin_port = htons(port);

    // build event base
    struct event_base* base = event_base_new();

    // set TCP_NODELAY to let data arrive at the server side quickly
    evutil_socket_t fd;
    fd = socket(AF_INET, SOCK_STREAM, 0);
    struct bufferevent* conn = bufferevent_socket_new(base, fd, BEV_OPT_CLOSE_ON_FREE);
    const char enable = 1;
    if (setsockopt(fd, IPPROTO_TCP, TCP_NODELAY,&enable, sizeof(enable)) < 0)
        printf("ERROR: TCP_NODELAY SETTING ERROR!\n");
    bufferevent_setcb(conn, read_cb, write_cb, bufferevent_cb, NULL); // For client, we don't need callback function
    bufferevent_enable(conn, EV_WRITE | EV_READ);
    if (bufferevent_socket_connect(conn, (struct sockaddr*)&my_address, sizeof(my_address)) == 0)
        printf("connect success\n");

    // start to send data
    bufferevent_write(conn, mesg, sizeof(mesg));
    // check the output evbuffer
    struct evbuffer* output = bufferevent_get_output(conn);
    int len = 0;
    len = evbuffer_get_length(output);
    printf("output buffer has %d bytes left\n", len);

    event_base_dispatch(base);

    bufferevent_free(conn);
    event_base_free(base);

    printf("Client program is over\n");

    return 0;
}