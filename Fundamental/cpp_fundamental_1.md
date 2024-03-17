# 구조체

- 구조체는 연관 있는 데이터를 묶을 수 있는 문법적 장치이다.

- C++에서는 구조체 내에 함수를 선언할 수 있게 허용하기 때문에 특정 구조체에 종속적인 함수들을 모아놓을 수 있다.

- 특정 구조체 객체가 여러 개 생성되더라도 모든 객체는 하나의 함수를 공유한다.

- 구조체 내에서만 유효한 상수는 enum을 이용해서 정의할 수 있다. 몇몇 구조체들 사이에서만 사용되는 상수들을 선언할 때는 이름공간을 이용해서 상수가 사용되는 영역을 명시하는 방법도 있다.

<details><summary>ex</summary>
    
```cpp
struct Monster
{
    enum
    {
        NAME_LEN     = 20,
        MAX_STRENGTH = 100,
    };
};

namespace STAT
{
    enum
    {
        HP  = 0,
        ATK = 1,
    }
}

int main(void)
{
    Monster monster;
    std::cout << "Name length: " << Monster::NAME_LEN << '\n';  // output: "Name length: 20"
    std::cout << "Name length: " << monster::NAME_LEN << '\n';  // output: "Name length: 20"
    std::cout << "HP: " << STAT::HP << '\n';                    // output: "HP: 0"

    return 0;
}
```

</details>

- 구조체 안에 함수가 정의되어 있으면 인라인으로 처리하라는 의미가 내포되어 있다. 따라서 함수를 구조체 밖으로 빼내는 경우 인라인으로 처리하라는 의미가 사라진다. 인라인을 유지하려면 명시적으로 inline 키워드를 추가해야 한다.

<details><summary>ex</summary>
    
```cpp
inline void Monster::Attack() { }
```

</details>

# 클래스

- 클래스와 구조체의 디폴트 접근제어 지시자는 각각 private, public이다.

<details><summary>ex</summary>

```cpp
struct Monster
{
    // 구조체의 디폴트 접근제어 지시자는 public이다.
    char name[10];
    int  hp;
};

class Player
{
    // 클래스의 디폴트 접근제어 지시자는 private이다.
    char name[10];
    int  hp;
public:
    Player(const char* name, int hp) : hp(hp)
    {
        strcpy(this->name, name);
    }

    void ShowInfo() const
    {
        std::cout << "Player(" << name << ", " << hp << ")" << std::endl;
    }
};

class Npc
{
public:
    char name[10];
    int  hp;
    
    Npc(const char* name, int hp) : hp(hp)
    {
        strcpy(this->name, name);
    }
};

int main(void)
{
    Monster monster = { "Goblin", 100 };
    std::cout << "Monster(" << monster.name << ", " << monster.hp << ")" << std::endl;

    //Player player = { "Knight", 50 };      // X, 컴파일 에러: private 멤버는 클래스 외부에서 접근할 수 없다.
    //Player player = Player("Knight", 50);  // O
    Player player("Knight", 50);             // O
    
    //std::cout << "Player(" << player.name << ", " << player.hp << ")" << std::endl;  // X, 컴파일 에러: private멤버는 클래스 외부에서 접근할 수 없다.
    player.ShowInfo();

    //Npc npc = { "Merchant", 25 };  // O
    Npc npc("Merchant", 25);         // O
    std::cout << "Npc(" << npc.name << ", " << npc.hp << ")" << std::endl;

    return 0;
}
```

</details>

- 클래스의 선언과 정의를 각각 별도의 소스파일로 구분한다.

<details><summary>ex</summary>

```cpp
// Monster.h 헤더파일에 클래스를 선언한다.
class Monster
{
private:
    char name[10];
    int  hp;
    int  mp;
public:
    void Attack();
    void Heal();
    void ShowInfo();
};

// 인라인 함수는 컴파일 과정에서 함수의 호출부를 함수의 몸체로 대체해야 하므로 헤더파일에 정의돼야 한다.
// 컴파일러는 파일 단위로 컴파일하므로 A.cpp, B.cpp를 동시에 컴파일해서 하나의 실행파일을 만든다 해도 A.cpp의 컴파일 과정에서 B.cpp를 참조하지 않으며 그 반대도 마찬가지다.
inline Monster::ShowInfo()
{
    std::cout << name << ", " << hp << ", " << mp << std::endl;
}

// Monster에서 제한적으로 사용되는 상수는 헤더파일에 선언한다.
namespace MONSTER_CONST
{
    enum
    {
        STRENGTH = 100, SPEED = 50
    };
}

// Monster.cpp 소스파일에 클래스를 정의힌다.
#include "Monster.h"  // 멤버함수의 정의 부분을 컴파일할 때 클래스의 멤버 변수나 헤더파일에 선언된 상수 등의 정보가 필요하다.

void Monster::Attack()
{
    std::cout << "Attack" << std::endl;
}
void Monster::Heal()
{
    std::cout << "Heal" << std::endl;
}
```

</details>

- 객체를 이루는 것은 데이터와 기능이다. 객체는 하나 이상의 상태 정보(데이터)와 하나 이상의 행동(기능)으로 구성된다.

- const 변수는 선언과 동시에 초기화돼야 한다.

<details><summary>ex</summary>

```cpp
class Monster
{
    //const int MAX_HP = 10;  // X, 컴파일 에러: 클래스의 멤버 변수 선언문에서는 초기화를 허용하지 않는다.
    const int MAX_HP;
public:
    Monster(int maxHp) : MAX_HP(maxHp) { }  // Member initializer는 선언(메모리 할당)과 동시에 초기화가 수행되므로 상수화된 멤버 변수를 초기화할 수 있다. 
};
```

</details>

- const 함수 내에서는 멤버 변수의 값을 변경할 수 없다.

<details><summary>ex</summary>

```cpp
class Monster
{
private:
    int hp;
public:
    int GetHp()
    {
        return hp;
    }

    void ShowHp() const
    {
        std::cout << "Hp: " << GetHp() << std::endl;  // 컴파일 에러: const 함수 내에서는 const가 아닌 함수의 호출이 제한된다. 
    }
};

class Item
{
private:
    int hp;
public:
    void SetHp(const Monster& monster)
    {
        hp = monster.GetHp();  // 컴파일 에러: const 참조자를 대상으로 값의 변경 능력을 가진 함수의 호출을 허용하지 않는다. 에러를 해결하려면 Monster::GetHp() 함수를 const 함수로 선언해야 한다.
    }
```

</details>

- malloc() 함수는 클래스의 크기 정보만 바이트 단위로 전달하기 때문에 당연하게도 생성자를 호출하지 않는다.

- 객체의 생성 방법을 제한하고자 하는 경우 private 생성자 사용을 고려해볼 수 있다.

- 사용자가 생성자, 소멸자를 정의하지 않는 경우 디폴트 생성자, 디폴트 소멸자가 자동으로 삽입된다.

<details><summary>ex</summary>

```cpp
Monster* pMonster = new Monster;    // O
Monster* pMonster = new Monster();  // O
Monster monster;                    // O
Monster monster();                  // X, 컴파일 에러: 함수의 원형 선언과 구분되지 않으므로 허용하지 않는다.

int main(void)
{
    Monster monster();  // 함수의 원형은 지역적으로도 선언할 수 있다.
    Monster m = monster();
}

Monster monster()
{
    Monster monster;
    return mosnter;
}
```

</details>

- 객체의 생성과정
1. 메모리 공간의 할당
2. 이니셜라이저를 이용한 멤버 변수 초기화
3. 생성자의 몸체부분 실행

- 생성자 몸체에서 멤버 변수를 초기화하는 방법 대비 Member initializer의 이점은 아래와 같다.
1. 초기화의 대상을 명확히 인식할 수 있다.
2. 성능에 약간의 이점이 있다. 선언과 동시에 초기화가 이뤄지는 형태로 바이너리 코드가 생성되기 때문이다. 참고로 이를 이용해서 상수화된 변수나 참조자를 클래스의 멤버 변수로 선언 및 초기화할 수 있다.

- this 포인터를 이용해서 self-reference를 반환할 수 있다.

<details><summary>ex</summary>

```cpp
class SelfRef
{
private:
    int num;
public:
    SelfRef(int n) : num(n) { }

    SelfRef& Adder(int n)
    {
        num += n;
        return *this;
    }

    SelfRef& ShowTwoNumber()
    {
        std::cout << num << std::endl;
        return *this;
    }
};

int main(void)
{
    SelfRef obj(3);
    SelfRef &ref = obj.Adder(2);

    obj.ShowTwoNumber();  // 5
    ref.ShowTwoNumber();  // 5

    ref.Adder(1).ShowTwoNumber().Adder(2).ShowTwoNumber();  // 6  // 8
    return 0;
}
```
    
</details>

- C++ 스타일 초기화

<details><summary>ex</summary>

```cpp
int  num = 20;  // O
int& ref = num; // O
int  num(20);   // O
int& ref(num);  // O
```
    
</details>

- 복사 생성자를 정의하지 않으면 멤버 대 멤버 복사를 수행하는 디폴트 복사 생성자가 자동으로 삽입된다. 참고로 복사 생성자의 매개변수는 무한루프를 막기 위해 반드시 참조형이어야 한다.

<details><summary>ex</summary>

```cpp
class Foo
{
private:
    int num1;
    int num2;
public:
    Foo(int n) : num1(n1) { num2 = 0; }
    Foo(int n1, int n2) : num1(n1), num2(n2) { }
};

int main(void)
{
    Foo foo1(10, 20);
    Foo foo2 = foo1;    // 디폴트 복사 생성자 호출
    // Foo foo2(foo1);  // 위 코드는 암시적으로 이와 같이 변경된다. 암시적 변환을 막음으로써 코드의 명확성을 높이려면 explicit 키워드를 이용한다.
    Foo foo3 = 5;       // 전달인자가 하나인 생성자의 경우도 암시적 변환이 발생한다.
    return 0;
}
```

</details>

- 클래스의 멤버 변수가 힙 메모리 공간을 참조하는 경우 디폴트 복사 생성자의 얕은 복사가 문제가 될 수 있다. 이 경우 깊은 복사 방식의 복사 생성자를 정의한다. 

<details><summary>ex</summary>

```cpp
class Person
{
private:
    char* name;
    int   age;
public:
    Person(char* name, int age) : age(age)
    {
        this->name = new char[strlen(name) + 1];
        strcpy(this->name, name);
    }

    Person(const Person& copy) : age(copy.age)
    {
        name = new char[strlen(copy.name) + 1];
        strcpy(name, copy.name);
    }

    ~Person()
    {
        delete[] name;
    }
```
    
</details>

- 복사 생성자 호출 시점(=메모리 할당과 동시에 초기화가 이뤄지는 시점)
1. 기존에 생성된 객체를 이용해서 새로운 객체를 초기화하는 경우
2. Call-by-value 방식의 함수호출 과정에서 객체를 인자로 전달하는 경우
3. 객체를 반환하되, 참조형으로 반환하지 않는 경우. 참고로 함수가 갑을 반환하면, 별도의 메모리 공간이 할당되고 이 공간이 반환 값으로 초기화된다.

- 임시 객체가 생성된 위치에는 임시 객체의 참조 값이 반환된다.

<details><summary>ex1</summary>

```cpp
#include <iostream>

class Temporary
{
private:
    int num;
public:
    Temporary(int n) : num(n)
    {
        std::cout << "Create obj: " << num << std::endl;
    }

    ~Temporary()
    {
        std::cout << "Destroy obj: " << num << std::endl;
    }

    void ShowTempInfo()
    {
        std::cout << "Num: " << num << std::endl;
    }
};

int main(void)
{
    Temporary(100);                         // 임시 객체는 다음 행으로 넘어가면 즉시 소멸된다.
    std::cout << "************After make" << std::endl;

    Temporary(200).ShowTempInfo();          // 임시 객체는 다음 행으로 넘어가면 즉시 소멸된다.
    std::cout << "************After make" << std::endl;

    const Temporary &ref = Temporary(300);  // 참조자에 참조되는 임시 객체는 바로 소멸되지 않는다.
    std::cout << "************After make" << std::endl;

    return 0;
}
```

</details>

<details><summary>ex2</summary>

```cpp
#include <iostream>

class SoSimple
{
private:
    int num;
public:
    SoSimple(int n) : num(n)
    {
        std::cout << "New obj: " << this << std::endl;
    }

    SoSimple(const SoSimple& copy) : num(copy.num)
    {
        std::cout << "New Copy obj: " << this << std::endl;
    }

    ~SoSimple()
    {
        std::cout << "Destroy obj: " << this << std::endl;
    }
};

SoSimple SimpleFuncObj(SoSimple ob)
{
    std::cout << "Parm ADR: " << &ob << std::endl;
    return ob;
}

int main(void)
{
    SoSimple obj(7);
    SimpleFuncObj(obj);

    std::cout << std::endl;
    SoSimple tempRef = SimpleFuncObj(obj);  // 반환되는 임시 객체에 tempRef라는 이름을 할당하게 된다. 객체 생성 수를 줄여 효율성을 높이려는 목적.
    std::cout << "Return obj " << &tempRef << std::endl;
    return 0;
}
```
    
</details>

- 상수화된 객체는 const 멤버함수만 호출할 수 있다.
- 함수의 const 키워드 유무도 함수 오버로딩의 조건에 해당된다.
<details><summary>ex</summary>

```cpp
#include <iostream>

class Foo
{
private:
    int num;
public:
    Foo(int n) : num(n) { }
    void Bar()
    {
        std::cout << "Bar: " << num << std::endl;
    }

    void Bar() const
    {
        std::cout << "const Bar: " << num << std::endl;
    }
};

void TestFunc(const Foo &foo)
{
    foo.Bar();
}

int main(void)
{
    Foo foo1(5);
    const Foo foo2(10);

    foo1.Bar();
    foo2.Bar();

    TestFunc(foo1);
    TestFunc(foo2);
    return 0;
}
```

</details>

- friend 선언
    - 클래스 내부에서 다른 클래스, 전역함수, 멤버함수 등에 대해 friend를 선언하면 private 멤버에 대한 접근이 가능해진다.
    - friend 선언은 private, public 영역 상관없이 클래스 내부에만 선언되면 된다.

<details><summary>ex1</summary>

```cpp
#include <iostream>
#include <cstring>

class Girl;  // Girl이라는 이름이 클래스의 이름임을 알림.

class Boy
{
private:
    int height;
    friend class Girl;  // Girl 클래스에 대한 friend 선언. Girl이라는 클래스에 대한 선언도 포함하므로 5번째 줄의 클래스 선언은 생략할 수 있다.
public:
    Boy(int len) : height(len) { }
    void ShowYourFriendInfo(Girl &frn);
};

class Girl
{
private:
    char phNum[20];
public:
    Girl(const char* num)
    {
        std::strcpy(phNum, num);
    }
    void ShowYourFriendInfo(Boy& frn);
    friend class Boy;  // Boy 클래스에 대한 friend 선언
};

void Boy::ShowYourFriendInfo(Girl& frn)  // Girl 클래스에 멤버변수 phNum이 존재한다는 사실을 알아야하기 때문에 Girl 클래스 정의보다 뒤에 위치함.
{
    std::cout << "Her phone number: " << frn.phNum << std::endl;
}

void Girl::ShowYourFriendInfo(Boy& frn)
{
    std::cout << "His height: " << frn.height << std::endl;
}

int main(void)
{
    Boy boy(170);
    Girl girl("010-1234-5678");
    boy.ShowYourFriendInfo(girl);
    girl.ShowYourFriendInfo(boy);
    return 0;
}
```

</details>

<details><summary>ex2</summary>

```cpp
(...)
class Point;

class PointOP
{
    (...)
    Point PointAdd(const Point&, const Point&);
    (...)
}

class Point
{
    (...)
    friend Point PointOP::PointAdd(const Point&, const Point&);  // 멤버함수에 대한 friend 선언.
    friend void ShowPointPos(const Point&);                      // friend 선언과 함수 원형에 대한 선언이 동시에 이뤄진다.
    (...)
```

</details>

- C에서의 static
    - 전역변수에 선언된 static: 선언된 파일 내에서만 참조를 허용한다.
    - 함수 내에 선언된 static: 한 번만 초기화되고, 지역변수와 달리 함수를 빠져나가도 소멸하지 않는다.
- C++에서의 static
    - static 멤버변수: 클래스당 하나씩만 생성된다(=클래스 변수). 객체 생성여부와 상관없이 메모리 공간에 단 하나만 할당되어 공유된다. 

<details><summary>ex1</summary>

```cpp
class Foo
{
private:
    int n;
    static int cnt;
public:
    static int num;
    const static int NUM_A = 100;  // const static으로 선언된 멤버변수는 선언과 동시에 초기화할 수 있다. 이에 반해 const 멤버변수의 초기화는 이니셜라이저를 통해야만 한다.
    mutable int num2;              // mutable 키워드를 통해 const 함수에 대해 예외를 둘 수 있다.
public:
    Foo()
    {
        cnt++;  // private으로 선언된 클래스 변수는 해당 클래스의 객체들만 접근이 가능하다.
    }

    static void Bar()
    {
        n++;  // 컴파일 에러: static 멤버함수 내에서는 static 멤버변수와 static 멤버함수만 호출할 수 있다.
    }
}
int Foo::cnt = 0;  // 클래스 변수는 객체가 생성될 때 동시에 생성되는 변수가 아니고 이미 메모리 공간에 할당이 이뤄진 변수이기 때문에 생성자 내부가 아니라 클래스 바깥에서 초기화한다. cnt변수는 메모리 공간에 저장될 때 0으로 초기화된다.
int Foo::num = 0;

int main(void)
{
    Foo foo();
    std::cout << "Class variable(num): " << Foo::num << std::endl;  // public으로 선언된 클래스 변수는 클래스 외부에서 클래스명으로 접근 가능하다.
    std::cout << "Class variable(num): " << foo::num << std::endl;  // 객체를 통해서도 접근할 수 있으나 이 방식은 멤버변수에 접근하는 것과 같은 오해를 불러일으키기 때문에 가급적 피한다.
    return 0;
}
```

</details>

- 상속 관계에서의 생성자, 소멸자
    - 유도 클래스의 객체생성 과정에서 기초 클래스의 생성자는 100% 호출된다.
    - 유도 클래스의 생성자에서 기초 클래스의 생성자 호출을 명시하지 않으면, 기초 클래스의 void 생성자가 호출된다.
    - 유도 클래스의 객체가 소멸될 때에는, 유도 클래스의 소멸자가 호출되고 난 다음에 기초 클래스의 소멸자가 호출된다.
    - 생성자에서 동적 할당한 메모리 공간은 소멸자에서 해제해야 한다.
- 객체 포인터 변수는 해당 클래스를 직접 혹은 간접적으로 상속하는 모든 객체를 가리킬 수 있다.
- 상속 관계인 클래스 간에 이름과 시그니처가 동일한 함수를 정의하는 것을 함수 오버라이딩이라고 한다. 함수가 오버라이딩 되면 오버라이딩 된 기초 클래스의 함수는 오버라이딩 한 유도 클래스의 함수에 가려진다.

<details><summary>ex1</summary>

```cpp
#include <iostream>
#include <cstring>

class Person
{
public:
    char* name;
public:
    Person(const char* myname)
    {
        name = new char[std::strlen(myname) + 1];
        std::strcpy(name, myname);
    }
    ~Person()
    {
        delete[] name;
    }
    void WhatYourName() const
    {
        std::cout << "My name is " << name << std::endl;
    }
};

class UnivStudent : public Person
{
private:
    char* major;
public:
    UnivStudent(const char* myname, const char* mymajor) : Person(myname)
    {
        major = new char[std::strlen(mymajor) + 1];
        std::strcpy(major, mymajor);
    }
    ~UnivStudent()
    {
        delete[] major;
    }
    void WhatYourName() const
    {
        std::cout << "It's " << name << std::endl;
    }
    void WhoAreYou() const
    {
        WhatYourName();
        Person::WhatYourName();
        std::cout << "My major is " << major << std::endl;
    }
};

int main(void)
{
    Person* st1 = new UnivStudent("Kim", "Mathmatics");
    st1->WhatYourName();
    std::cout << "-----------" << std::endl;
    UnivStudent st2("Hong", "Physics");
    st2.WhoAreYou();
    std::cout << "-----------" << std::endl;
    st2.Person::WhatYourName();

    delete st1;
    return 0;
}
```

</details>

<details><summary>ex2</summary>

```cpp
int main(void)
{
    Base* bptr = new Derived();
    bptr->DerivedFunc();             // 컴파일 에러: C++ 컴파일러는 포인터 연산의 가능성 여부를 판단할 때, 포인터의 자료형을 기준으로 판단하지, 실제 가리키는 객체의 자료형을 기준으로 판단하지 않는다.
    Derived* dptr = bptr;            // 컴파일 에러: 포인터 bptr의 포인터 형만을 가지고 대입의 가능성을 판단한다.

    Derived* dptr2 = new Derived();  // O
    Base* bptr2 = dptr2;             // O
    return 0;
}
```

</details>

- 가상함수는 포인터의 자료형을 기반으로 호출대상을 결정하지 않고, 포인터 변수가 실제로 가리키는 객체를 참조하여 호출의 대상을 결정한다.

<details><summary>ex</summary>

```cpp
#include <iostream>

class First
{
public:
    void FirstFunc() { std::cout << "FirstFunc" << std::endl; }
    void MyFunc() { std::cout << "FirstMyFunc" << std::endl; }
    virtual void SimpleFunc() { std::cout << "FirstSimpleFunc" << std::endl; }
};

class Second : public First
{
public:
    void SecondFunc() { std::cout << "SecondFunc" << std::endl; }
    void MyFunc() { std::cout << "SecondMyFunc" << std::endl; }
    virtual void SimpleFunc() { std::cout << "SecondSimpleFunc" << std::endl; }  // 오버라이딩 관계인 부모 함수에 virtual 키워드가 있다면 자식 클래스에서는 생략해도 자동으로 가상함수가 되지만 명시적으로 표현하기 위해 virtual 키워드 추가했음.
};

class Third : public Second
{
public:
    void ThirdFunc() { std::cout << "ThirdFunc" << std::endl; }
    void MyFunc() { std::cout << "ThirdMyFunc" << std::endl; }
    virtual void SimpleFunc() { std::cout << "ThirdSimpleFunc" << std::endl; }
};

int main(void)
{
    Third* tptr = new Third();
    Second* sptr = tptr;
    First* fptr = sptr;

    tptr->FirstFunc();    // O
    tptr->SecondFunc();   // O
    tptr->ThirdFunc();    // O

    sptr->FirstFunc();    // O
    sptr->SecondFunc();   // O
    //sptr->ThirdFunc();  // X

    fptr->FirstFunc();    // O
    //fptr->SecondFunc(); // X
    //fptr->ThirdFunc();  // X

    std::cout << "--------------------" << std::endl;

    fptr->MyFunc();
    sptr->MyFunc();
    tptr->MyFunc();
    
    std::cout << "--------------------" << std::endl;

    fptr->SimpleFunc();
    sptr->SimpleFunc();
    tptr->SimpleFunc();

    std::cout << "--------------------" << std::endl;

    Third obj;
    obj.FirstFunc();
    obj.SecondFunc();
    obj.ThirdFunc();
    obj.SimpleFunc();

    Second& sref = obj;
    sref.FirstFunc();
    sref.SecondFunc();
    sref.SimpleFunc();

    First& fref = obj;
    fref.FirstFunc();
    fref.SimpleFunc();

    delete tptr;
    return 0;
}
```

</details>

- 순수 가상함수란, 함수의 몸체가 정의되지 않은 함수를 의미한다. 순수 가상함수를 선언한 클래스를 추상 클래스라고 한다.
- 추상 클래스로 선언함으로써 잘못된 객체의 생성을 막을 수 있고, 유도 클래스에서의 오버라이드를 위한 더미 함수에 대해 명확하게 의도를 표시할 수 있다.

<details><summary>ex</summary>

```cpp
class Foo  // 추상 클래스
{
protected:
    virtual void Bar() = 0;  // 순수 가상함수
}
```

</details>

- 가상 소멸자

<details><summary>ex</summary>

```cpp
#include <iostream>

class First
{
private:
    char* strOne;
public:
    First(const char* str)
    {
        strOne = new char[std::strlen(str) + 1];
    }
    virtual ~First()
    {
        std::cout << "~First()" << std::endl;
        delete[] strOne;
    }
};

class Second : public First
{
private:
    char* strTwo;
public:
    Second(const char* str1, const char* str2) : First(str1)
    {
        strTwo = new char[std::strlen(str2) + 1];
    }
    ~Second()
    {
        std::cout << "~Second()" << std::endl;
        delete[] strTwo;
    }
};

int main(void)
{
    First* ptr = new Second("simple", "complex");
    delete ptr;  // First 클래스의 소멸자만 호출한다.
    return 0;
}
```

</details>

- 한 개 이상의 가상함수를 포함하는 클래스에 대해서는 컴파일러가 가상함수 테이블(virtual table, V-table)을 만든다. V-table에는 함수를 구분지어주는 구분자 key와 구분자에 해당하는 함수의 주소정보를 알려주는 value가 있다. 오버라이딩 된 가상함수의 주소정보는 유도 클래스의 가상함수 테이블에 포함되지 않는다. 따라서 오버라이딩 된 가상함수를 호출하면, 무조건 가장 마지막에 오버라이딩을 한 유도 클래스의 멤버함수가 호출된다.
- 가상함수 테이블은 멤버함수 호출에 사용되는 일종의 데이터이기 때문에 객체의 생성과 상관없이 main() 함수가 호출되기 이전에 메모리 공간에 할당된다.
- 다중상속의 모호성: 다중상속의 대상이 되는 두 기초 클래스에 동일한 이름의 멤버가 존재하는 경우 문제가 발생할 수 있다.

<details><summary>ex1</summary>

```cpp
#include <iostream>

class BaseOne
{
public:
    void SimpleFunc() { std::cout << "BaseOne" << std::endl; }
};

class BaseTwo
{
public:
    void SimpleFunc() { std::cout << "BaseTwo" << std::endl; }
};

class MultiDerived : public BaseOne, protected BaseTwo
{
public:
    void ComplexFunc()
    {
        BaseOne::SimpleFunc();  // 어느 클래스에 정의된 함수를 호출할 것인지 명시해야 한다.
        BaseTwo::SimpleFunc();
    }
};

int main(void)
{
    MultiDerived mdr;
    mdr.ComplexFunc();
    return 0;
} 
```

</details>

<details><summary>ex2</summary>

```cpp
#include <iostream>

class Base
{
public:
    Base() { std::cout << "Base ctor" << std::endl; }
    void SimpleFunc() { std::cout << "BaseOne" << std::endl; }
};

class MiddleDerivedOne : virtual public Base
{
public:
    MiddleDerivedOne() : Base() { std::cout << "MiddleDerivedOne ctor" << std::endl; }
    void MiddleFuncOne()
    {
        SimpleFunc();
        std::cout << "MiddleDerivedOne" << std::endl;
    }
};

class MiddleDerivedTwo : virtual public Base
{
public:
    MiddleDerivedTwo() : Base() { std::cout << "MiddleDerivedTwo ctor" << std::endl; }
    void MiddleFuncTwo()
    {
        SimpleFunc();
        std::cout << "MiddleDerivedTwo" << std::endl;
    }
};

class LastDerived : public MiddleDerivedOne, public MiddleDerivedTwo
{
public:
    LastDerived() : MiddleDerivedOne(), MiddleDerivedTwo() { std::cout << "LastDerived ctor" << std::endl; }
    void ComplexFunc()
    {
        MiddleFuncOne();
        MiddleFuncTwo();
        SimpleFunc();  // 가상상속이 아니었다면 MiddleDerivedOne::SimpleFunc() 또는 MiddleDerivedTwo::SimpleFunc()으로 명시했어야 하며 Base클래스의 생성자도 두 번 호출된다.
    }
};

int main(void)
{
    std::cout << "객체 생성 전....." << std::endl;
    LastDerived ldr;
    std::cout << "객체 생성 후....." << std::endl;
    ldr.ComplexFunc();
    return 0;
}
```

</details>

## Reference

- 윤성우, <열혈 C++ 프로그래밍>
