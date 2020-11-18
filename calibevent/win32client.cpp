#include <winsock2.h>
#pragma comment(lib , "Ws2_32.lib")

#include <Windows.h>

//等待接收操作
//参数1：套接字描述符 参数2：等待的秒数
BOOL WaitRecv(SOCKET s, int timeout) {
	fd_set nfds;
	RtlZeroMemory(&nfds, sizeof(fd_set));
	nfds.fd_array[0] = s;
	nfds.fd_count = 1;
	timeval time = { timeout, 0 };
	if (select(0, &nfds, NULL, NULL, &time) <= 0) {
		return FALSE;
	}
	return TRUE;
}


//int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int  nCmdShow)
int main3(int argc, char** argv)
{
	WORD wVersionRequested;
	WSADATA wsaData;
	wVersionRequested = MAKEWORD(2, 2);

	SOCKET Socket;

	sockaddr_in addr;

	if (WSAStartup(wVersionRequested, &wsaData) != 0) {
		MessageBox(NULL, TEXT("Winsock开启失败"), TEXT("错误"), MB_ICONERROR);
		return 1;
	}

	Socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (Socket == INVALID_SOCKET)
	{
		MessageBox(NULL, TEXT("Socket创建失败"), TEXT("错误"), MB_ICONERROR);
		return 1;
	}

	addr.sin_family = AF_INET;
	addr.sin_port = htons(8080);
	addr.sin_addr.S_un.S_addr = inet_addr("127.0.0.1");//服务器地址


	//setsockopt(,,);

	while(connect(Socket, (sockaddr*)&addr, sizeof(sockaddr_in)) == SOCKET_ERROR)
	{
		Sleep(100);
		//MessageBox(NULL, TEXT("Socket连接到服务器失败"), TEXT("错误"), MB_ICONERROR);
		//exit(1);
	}

	send(Socket, "2@", 2, 0);

	char msg[1001] = { 0 };
	int readlen = 0;
	int addrlen = sizeof(sockaddr_in);
	RtlZeroMemory(msg, 1001);

	if (!WaitRecv(Socket, 12)) {//等待接收
		closesocket(Socket);
		return 0;
	}

	int r = recv(Socket, msg, 1000, 0);
	int len = 0;
	if (r > 0) {
		unsigned int x = (unsigned int)strstr(msg, "@");//找到第一个出现@的位置，按照预先的协议，@之前是完整的数据长度
		if (x != 0) {
			msg[x - (unsigned int)msg] = '\0';//将@换成字符串结束符
			len = atoi(msg);//获取长度
			msg[x - (unsigned int)msg] = '@';//替换回来
			readlen = r;//已经读取的字节
			MessageBoxA(NULL, msg, "收到数据", 0);

			while (readlen < len) {//一直接收，直到达到我们认为的长度
				RtlZeroMemory(msg, 1001);
				if (!WaitRecv(Socket, 2))break;//超时
				r = recv(Socket, msg, 1000, 0);
				if (r <= 0)break;
				readlen += r;
				MessageBoxA(NULL, msg, "收到数据", 0);
			}
		}
		else {
			closesocket(Socket);
		}
	}
	MessageBoxA(NULL, "数据接收完成", "", 0);
	closesocket(Socket);

	return 0;
}