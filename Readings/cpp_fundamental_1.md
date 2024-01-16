# 구조체

- 구조체는 연관 있는 데이터를 묶을 수 있는 문법적 장치이다.
- C++에서는 구조체 내에 함수를 선언할 수 있게 허용하기 때문에 특정 구조체에 종속적인 함수들을 모아놓을 수 있다.
- 특정 구조체 객체가 여러 개 생성되더라도 모든 객체는 하나의 함수를 공유한다.
- 구조체 내에서만 유효한 상수는 enum을 이용해서 정의할 수 있다. 몇몇 구조체들 사이에서만 사용되는 상수들을 선언할 때는 이름공간을 이용해서 상수가 사용되는 영역을 명시하는 방법도 있다.
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
- 구조체 안에 함수가 정의되어 있으면 인라인으로 처리하라는 의미가 내포되어 있다. 따라서 함수를 구조체 밖으로 빼내는 경우 인라인으로 처리하라는 의미가 사라진다. 인라인을 유지하려면 명시적으로 inline 키워드를 추가해야 한다.
```cpp
inline void Monster::Attack() { }
```

# 클래스

- 클래스와 구조체의 디폴트 접근제어 지시자는 각각 private, public이다.
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

- 클래스의 선언과 정의를 각각 별도의 소스파일로 구분한다.
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
- 객체를 이루는 것은 데이터와 기능이다. 객체는 하나 이상의 상태 정보(데이터)와 하나 이상의 행동(기능)으로 구성된다.
- const 변수는 선언과 동시에 초기화돼야 한다.
```cpp
class Monster
{
    //const int MAX_HP = 10;  // X, 컴파일 에러: 클래스의 멤버 변수 선언문에서는 초기화를 허용하지 않는다.
    const int MAX_HP;
public:
    Monster(int maxHp) : MAX_HP(maxHp) { }  // Member initializer는 선언(메모리 할당)과 동시에 초기화가 수행되므로 상수화된 멤버 변수를 초기화할 수 있다. 
};
```
- const 함수 내에서는 멤버 변수의 값을 변경할 수 없다.
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


## Reference

- 윤성우, <열혈 C++ 프로그래밍>
