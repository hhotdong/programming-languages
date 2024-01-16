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

- 클래스와 구조체의 접근제어 지시자
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


## Reference

- 윤성우, <열혈 C++ 프로그래밍>
