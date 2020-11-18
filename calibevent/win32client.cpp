#include <winsock2.h>
#pragma comment(lib , "Ws2_32.lib")

#include <Windows.h>

//�ȴ����ղ���
//����1���׽��������� ����2���ȴ�������
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
		MessageBox(NULL, TEXT("Winsock����ʧ��"), TEXT("����"), MB_ICONERROR);
		return 1;
	}

	Socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (Socket == INVALID_SOCKET)
	{
		MessageBox(NULL, TEXT("Socket����ʧ��"), TEXT("����"), MB_ICONERROR);
		return 1;
	}

	addr.sin_family = AF_INET;
	addr.sin_port = htons(8080);
	addr.sin_addr.S_un.S_addr = inet_addr("127.0.0.1");//��������ַ


	//setsockopt(,,);

	while(connect(Socket, (sockaddr*)&addr, sizeof(sockaddr_in)) == SOCKET_ERROR)
	{
		Sleep(100);
		//MessageBox(NULL, TEXT("Socket���ӵ�������ʧ��"), TEXT("����"), MB_ICONERROR);
		//exit(1);
	}

	send(Socket, "2@", 2, 0);

	char msg[1001] = { 0 };
	int readlen = 0;
	int addrlen = sizeof(sockaddr_in);
	RtlZeroMemory(msg, 1001);

	if (!WaitRecv(Socket, 12)) {//�ȴ�����
		closesocket(Socket);
		return 0;
	}

	int r = recv(Socket, msg, 1000, 0);
	int len = 0;
	if (r > 0) {
		unsigned int x = (unsigned int)strstr(msg, "@");//�ҵ���һ������@��λ�ã�����Ԥ�ȵ�Э�飬@֮ǰ�����������ݳ���
		if (x != 0) {
			msg[x - (unsigned int)msg] = '\0';//��@�����ַ���������
			len = atoi(msg);//��ȡ����
			msg[x - (unsigned int)msg] = '@';//�滻����
			readlen = r;//�Ѿ���ȡ���ֽ�
			MessageBoxA(NULL, msg, "�յ�����", 0);

			while (readlen < len) {//һֱ���գ�ֱ���ﵽ������Ϊ�ĳ���
				RtlZeroMemory(msg, 1001);
				if (!WaitRecv(Socket, 2))break;//��ʱ
				r = recv(Socket, msg, 1000, 0);
				if (r <= 0)break;
				readlen += r;
				MessageBoxA(NULL, msg, "�յ�����", 0);
			}
		}
		else {
			closesocket(Socket);
		}
	}
	MessageBoxA(NULL, "���ݽ������", "", 0);
	closesocket(Socket);

	return 0;
}