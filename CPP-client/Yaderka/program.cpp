#define _WINSOCK_DEPRECATED_NO_WARNINGS
#include <winsock2.h>
#include <windows.h>
#include <iostream>
#pragma comment(lib, "ws2_32.lib")

using namespace std;

void language();
void chooseAction();
void clear();
void startSock();
void connectToServer();
void sendMessage();
void messageFromServer();
void resultMessage();
void closeSock();


string key;

SOCKET id;

int main()
{
	
	startSock();
	connectToServer();

	language();

	chooseAction();

	return 0;
}

 void language()
{
	SetConsoleCP(1251);
	SetConsoleOutputCP(1251);
}

 void chooseAction()
 {
	 char action;
	 bool valid_choise = true;

	 do
	 {
		 cout << "Key client\n";
		 cout << "--------------\n\n";
		 cout << "Choose action\n";
		 cout << "1. Get key\n";
		 cout << "2. Validate key\n";

		 cin >> action;

		 clear();
		 switch (action)
		 {
		 case '1':
			 cout << "key: ";
			 messageFromServer();
			 getchar();
			 getchar();

			 clear();

			 break;

		 case '2':
			 cout << "input key:";
			 cin >> key;

			 sendMessage();
			 cout << endl;
			 resultMessage();
		
			 getchar();
			 getchar();
			 clear();
			 break;

		 }
	 } while (valid_choise);
 }

 void clear()
 {
	 system("cls");
 }

 void startSock()
 {
	 WSADATA box;

	 WSAStartup(MAKEWORD(2, 2), &box);

 }

 void connectToServer()
 {
	  id = socket(AF_INET, SOCK_STREAM, 0);

	 sockaddr_in server;

	 server.sin_family = AF_INET;
	 server.sin_port = htons(8686);                       // port
	 server.sin_addr.s_addr = inet_addr("192.168.1.77"); // IP 

	 connect(id, (sockaddr*)&server, sizeof(server));
 }

 void sendMessage()
 {
	 send(id, key.c_str(), (int)key.size(), 0);
 }

 void messageFromServer()
 {
	 char buffer[1024] = {};
	 recv(id, buffer, sizeof(buffer), 0);
	 cout  << buffer;
 }

 void closeSock()
 {
	 closesocket(id);
	 WSACleanup();
 }

 void resultMessage()
 {
	 char buffer[1024] = {};
	 recv(id, buffer, sizeof(buffer), 0);

	 switch (*buffer)
	 {
		 case '0':
			 cout << "Validation failed";
			 break;

		 case '1':
			 cout << "Validation succesful";
			 break;
	 }
 }