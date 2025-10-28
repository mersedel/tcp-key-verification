#define _CRT_SECURE_NO_WARNINGS

#include<stdio.h>
#include<windows.h>

int code=0;

void choose_optionf(void);

void language(void);
void clear(void);


int main()
{
	language();

	choose_optionf();
	
	getchar();
	getchar();
	
	return 0;
}



 void language()
{
	 SetConsoleCP(1251);
	 SetConsoleOutputCP(1251);
}

 void choose_optionf()
 {
	 char key [17],final_key[33];

	 int operation;
	 int wrong_operation = 1;

	
	 do
	 {
		 clear();

		 printf("nuclear client\n");
		 printf("--------------\n\n");
		 printf("Вибери дію:\n");
		 printf("1 ввести фрагмент ключа\n");
		 printf("2 отримати ключ\n");
		 printf("3 ввести ключ запуску \n");

		 scanf("%d", &operation);

		 clear();

		 switch (operation)
		 {
		 case 1:
			 code = 1;
			 printf("Введи ключ\n");
			 scanf("%16s", key);

			 wrong_operation = 0;
			 // відправити на сервер key[16]
			 break;

		 case 2:
			 code = 0;
			 printf("ключ:");

			 wrong_operation = 0;
			 //відправка на сервер коду щоб отримати пароль 

			 break;
		 case 3:
			 code = 2;
			 printf("ключ:\n");
			 scanf("%32s", final_key);

			 wrong_operation = 0;
			 //відправка на сервер коду

			 break;

		

		 }
	 } while (wrong_operation == 1);
 }
 void clear()
 {
	 system("cls");
 }
 // code = 0 відправка на сервер коду щоб отримати пароль 
 // code = 1 відправка масиву на сервер 
 // code = 2 відправка на сервер фінального пароля 