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

//cmd 类型的
enum MainType
{
	CmdNull = -1,
	CmdControl = 0x0001, //控制
	CmdSearch = 0x0002,  //查询
	CmdNotify = 0x0003,  //通知
};

//控制类型
enum ControlType
{
	Trigger,
	Inc,
	Dec,
	Level,
	Value,
};

//查询类型
enum QueryType
{
	Query,
	List,
	Enable,
	Disable,
};

//查询数据
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

//查询数据2
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

///包头定义
struct PackageHead
{
	//  主机标识
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

