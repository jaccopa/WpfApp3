#pragma once
#include <winsock2.h>
#pragma comment(lib , "Ws2_32.lib")
#include <Mmsystem.h>
#pragma comment(lib, "winmm.lib")
#include <Windows.h>
#include <process.h>
#include <thread>
#include <string>
#include "lockFreeQueue.h"

using namespace std;


enum CmdType {
	Null = -1,
	CmdStr = 0x0001,
	Iod = 0x0002,
	CmdByte = 0x0003,
	CmdCmd = 0x0004,
};

//cmd ���͵�
enum MainType
{
	CmdNull = -1,
	CmdControl = 0x0001, //����
	CmdSearch = 0x0002,  //��ѯ
	CmdNotify = 0x0003,  //֪ͨ
};

//��������
enum ControlType
{
	Trigger,
	Inc,
	Dec,
	Level,
	Value,
};

//��ѯ����
enum QueryType
{
	Query,
	List,
	Enable,
	Disable,
};

//��ѯ����
struct QueryParaData
{
	string ControlType;
	string ParaType;
	string ParaName;
	int Level;
	bool IsEnable;
	bool IsActive;
	float Value;
};

//��ѯ����2
struct QueryParaData2
{
	string ControlType;
	string ParaType;
	string ParaName;
	int Level;
	bool IsEnable;
	bool IsActive;
	string Value;
};

///��ͷ����
struct PackageHead
{
	//  ������ʶ
	int  HostHandle;
	//FlowID
	unsigned short FlowID;
	CmdType Type;
	MainType MainType;
	int Length;
	int StartSeqNO;
};

struct MyStructData
{
	int len;
	char* pdata;
};

class tclient
{
public:
	tclient(const char* serverip, const int serverport);
	bool Connect();
	void DisConnect();

	void PushCmd(const char* cmd, const int len);

	atomic<bool> Alive;



	void Recv();

	void Send();

	~tclient();

private:
	 char* mServerip;
	 int mServerport;

	int sendFailedCt;
	bool Init(const char* serverip, const int serverport);
	char* pBuf;
	const int m_sockMaxBufSize = 1024 * 64;
	SOCKET		Socket;
	sockaddr_in addr;
	CLockFreeQueue<MyStructData*, 1000>  cmdQ;
	bool m_bReceive;

	int m_RevDataLength;
	PackageHead m_PackHead;
	int m_RevBufLength;
};

