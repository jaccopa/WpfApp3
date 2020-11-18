/*
  This example program provides a trivial server program that listens for TCP
  connections on port 9995.  When they arrive, it writes a short message to
  each client connection, and closes each connection once it is flushed.

  Where possible, it exits cleanly in response to a SIGINT (ctrl-c).
*/

#pragma comment(lib,"ws2_32.lib")

#include <string.h>
#include <errno.h>
#include <stdio.h>
#include <signal.h>
#ifndef _WIN32
#include <netinet/in.h>
# ifdef _XOPEN_SOURCE_EXTENDED
#  include <arpa/inet.h>
# endif
#include <sys/socket.h>
#endif

#include <event2/bufferevent.h>
#include <event2/buffer.h>
#include <event2/listener.h>
#include <event2/util.h>
#include <event2/event.h>
#include <event2/event.h>
#include <event2/bufferevent.h>


struct event_base* base;
struct bufferevent* bev;
struct sockaddr_in sin;


static const char MESSAGE[] = "Hello, World!\n";


void Connect()
{
    if (bufferevent_socket_connect(bev,
        (struct sockaddr*)&sin, sizeof(sin)) < 0) {
        /* Error starting connection */
        //bufferevent_free(bev);
        //return -1;
    }
}

void SendToServer(struct bufferevent* bev)
{
    static const char MESSAGE[] = "Hello, World!\n";
    bufferevent_write(bev, MESSAGE, strlen(MESSAGE));
}

void eventcb(struct bufferevent* bev, short events, void* ptr)
{
    if (events & BEV_EVENT_CONNECTED) {
        /* We're connected to 127.0.0.1:8080.   Ordinarily we'd do
           something here, like start reading or writing. */
        printf_s("connected...\r\n");
        SendToServer(bev);
    }
    else if (events & BEV_EVENT_ERROR) {
        /* An error occured while connecting. */
        printf_s("error...\r\n");

        Connect();
    }
}

void conn_writecb(struct bufferevent* bev, void* user_data)
{
    struct evbuffer* output = bufferevent_get_output(bev);
    if (evbuffer_get_length(output) == 0) {
        printf("flushed answer\n");
        bufferevent_free(bev);
    }

}



void readcb(struct bufferevent* bev, void* ptr)
{
    char buf[1024];
    int n;
    struct evbuffer* input = bufferevent_get_input(bev);
    while ((n = evbuffer_remove(input, buf, sizeof(buf))) > 0) {
        //fwrite(buf, 1, n, stdout);
    }
}


int main(int argc, char** argv)
{
#ifdef _WIN32
    WSADATA wsa_data;
    WSAStartup(0x0201, &wsa_data);
#endif

    base = event_base_new();

    memset(&sin, 0, sizeof(sin));
    sin.sin_family = AF_INET;
    sin.sin_addr.s_addr = htonl(0x7f000001); /* 127.0.0.1 */
    sin.sin_port = htons(8080); /* Port 8080 */

    bev = bufferevent_socket_new(base, -1, BEV_OPT_CLOSE_ON_FREE);

    bufferevent_setcb(bev, readcb, NULL, eventcb, NULL);

    bufferevent_enable(bev, EV_WRITE | EV_READ);

    Connect();

    

    event_base_dispatch(base);
    //event_base_free(base);

    return 0;
}