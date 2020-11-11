#pragma once
#include <winsock2.h>
#pragma comment(lib , "Ws2_32.lib")
#include <Mmsystem.h>
#pragma comment(lib, "winmm.lib")
#include <Windows.h>

class tclient
{
public:
	tclient(const char* serverip, const int serverport);
	bool Connect();
	void DisConnect();

	void PushCmd(const char* cmd, const int len);

	~tclient();

private:

	bool Init(const char* serverip, const int serverport);

	void Send();

	void Recv();


	SOCKET		Socket;
	sockaddr_in addr;
	HANDLE      thRecv;
	HANDLE      thSend;
	MMRESULT	timer_id;
};

