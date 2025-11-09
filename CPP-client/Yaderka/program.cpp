#include <iostream>
#include <windows.h>
#include <string>

using namespace std;

void language();
void chooseAction();
void clear();


string key;

int main()
{
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
		 cout << "Nucler client\n";
		 cout << "--------------\n\n";
		 cout << "вибери дію\n";
		 cout << "1.отримати ключ\n";
		 cout << "2.відправити ключ\n";

		 cin >> action;

		 clear();
		 switch (action)
		 {
		 case '1':
			 cout << "наш ключ";
			 
			 getchar();
			 getchar();

			 clear();

			 break;

		 case '2':
			 cin >> key;

			 clear();

			 break;

		 }
	 } while (valid_choise);
	 

 }

 void clear()
 {
	 system("cls");
 }
