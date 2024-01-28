# 연산자 오버로딩

- foo1 + foo2
  - 멤버함수로 오버로딩한 경우 => foo1.operator+(foo2)
  - 전역함수로 오버로딩한 경우 => operator+(foo1, foo2)
  - 동일한 자료형을 대상으로 +연산자를 멤버함수와 전역함수로 각각 오버로딩한 경우 멤버함수가 우선시된다. 그러나 일부 오래된 컴파일러에서는 에러가 발생하기도 하므로 가급적 피해야 한다.

- ++foo
  - 멤버함수로 오버로딩한 경우 => pos.operator++()
  - 전역함수로 오버로딩한 경우 => operator++(pos)

<details><summary>ex</summary>

```cpp
#include <iostream>

class Point
{
private:
    int xpos, ypos;
public:
    Point(int x = 0, int y = 0) : xpos(x), ypos(y) { std::cout << "ctor" << std::endl; }
  
    void ShowPosition() const
    {
        std::cout << '[' << xpos << ", " << ypos << ']' << std::endl;
    }
  
    Point& operator++()
    {
        xpos += 1;
        ypos += 1;
        return *this;
    }

    const Point operator++(int)  // 후위증가. 반환형이 const라는 건 임시 객체를 const 객체(=상수 객체)로 반환하겠다는 의미힘.
    {
        const Point retobj(*this);  // 함수 내에서 retobj의 변경을 막는다는 의미. 반환형이 const인 것과는 관계없음.
        xpos += 1;
        ypos += 1;
        return retobj;
    }

    friend Point operator+(const Point& pos1, const Point& pos2);
    friend Point& operator--(Point& ref);
    friend const Point operator--(Point& ref, int);  // 후위감소
};

Point operator+(const Point& pos1, const Point& pos2)
{
    return Point(pos1.xpos + pos2.xpos, pos1.ypos + pos2.ypos);
}

Point& operator--(Point& ref)
{
    ref.xpos -= 1;
    ref.ypos -= 1;
    return ref;
}

const Point operator--(Point& ref, int)
{
    const Point retobj(ref);
    ref.xpos -= 1;
    ref.ypos -= 1;
    return retobj;
}

int main(void)
{
    Point p1(1,2);
    Point p2(3,4);
    Point p3 = p1 + p2;

    p1.ShowPosition();
    p2.ShowPosition();
    p3.ShowPosition();

    std::cout << "---------------------" << std::endl;
    
    ++(++p1);
    --(--p2);

    p1.ShowPosition();
    p2.ShowPosition();

    std::cout << "---------------------" << std::endl;

    Point cpy;
    cpy = p1--;
    cpy.ShowPosition();
    p1.ShowPosition();

    cpy = p1++;
    cpy.ShowPosition();
    p1.ShowPosition();

    std::cout << "---------------------" << std::endl;
    
    const Point p4;
    const Point& ref = p4;  // 컴파일 OK. const 객체를 대상으로 값의 변경능력을 지니는 함수 호출은 허용되지 않는다. 따라서 const 객체를 대상으로 참조자를 선언할 떄에는 참조자도 const로 선언해야 한다.

    //(p1++)++;  // 컴파일 에러. 상수 객체를 대상으로는 const로 선언되지 않은 멤버함수 호출이 불가능하다.

    return 0;
}
```

</details>

- 멤버함수 기반으로만 오버로딩이 가능한 연산자(=객체를 대상으로 진행해야 의미가 있는 연산자)
  - 대입연산자 '='
  - 함수 호출 연산자 '()'
  - 배열 접근 연산자 '[]'
  - 멤버 접근을 위한 포인터 연산자 '->'

- 교환법칙 문제의 해결을 위한 전역함수 오버로딩

<details><summary>ex</summary>

```cpp
#include <iostream>

class Point
{
private:
    int xpos, ypos;
public:
    Point(int x = 0, int y = 0) : xpos(x), ypos(y) { }
    void ShowPosition() const
    {
        std::cout << '[' << xpos << ", " << ypos << ']' << std::endl;
    }
    Point operator*(int times)
    {
        return Point(xpos * times, ypos * times);
    }
    friend Point operator*(int times, Point& pos);
};

Point operator*(int times, Point& pos)
{
    return pos * times;
}

int main(void)
{
    Point pos(1, 2);
    Point cpy;

    cpy = 3 * pos;
    cpy.ShowPosition();

    cpy = 2 * pos * 3;
    cpy.ShowPosition();

    return 0;
}
```

</details>

- cin, cout, endl

<details><summary>ex</summary>

```cpp
#include <iostream>

namespace mystd
{
    using namespace std;  // mystd 내에서 지역적으로 이뤄진 선언이므로 이 지역 내에서만 유효하다.

    class ostream
    {
    public:
        ostream& operator<< (const char* str)
        {
            printf("%s", str);
            return *this;
        }
        ostream& operator<< (char str)
        {
            printf("%c", str);
            return *this;
        }
        ostream& operator<< (int num)
        {
            printf("%d", num);
            return *this;
        }
        ostream& operator<< (double e)
        {
            printf("%g", e);
            return *this;
        }
        ostream& operator<< (ostream& (*fp)(ostream& ostm))
        {
            fp(*this);
            return *this;
        }
    };

    ostream& endl(ostream& ostm)  // endl은 함수의 이름이다. 버퍼를 비우는 작업도 함께 수행한다.
    {
        ostm << '\n';
        fflush(stdout);
        return ostm;
    }

    ostream cout;  // cout은 ostream 클래스의 객체 이름이다.
}

int main(void)
{
    // main()함수 내에서는 cout, endl이 이름공간 mystd 내에 선언된 것을 의미한다.
    using mystd::cout;
    using mystd::endl;
    cout << "Simple String" << endl << 3.14 << endl << 123 << endl;

    return 0;
}
```

</details>

- 사용자 정의 타입에 대한 <<, >> 연산자 오버로딩을 위한 전역함수 오버로딩
  - cout은 ostream 클래스의 객체이다.
  - ostream은 이름공간 std 안에 선언되어 있다.
  - 멤버함수를 통해 오버로딩을 하려면 ostream 클래스를 수정해야 하는데 이는 불가능하므로 전역함수 방식을 이용한다.

<details><summary>ex</summary>

```cpp
#include <iostream>

class Point
{
private:
    int xpos, ypos;
public:
    Point(int x = 0, int y = 0) : xpos(x), ypos(y) { }
    void ShowPosition() const
    {
        std::cout << '[' << xpos << ", " << ypos << ']' << std::endl;
    }
    friend std::ostream& operator<<(std::ostream&, const Point&);
};

std::ostream& operator<<(std::ostream& ostm, const Point& pos)
{
    ostm << '[' << pos.xpos << ", " << pos.ypos << ']' << std::endl;
    return ostm;
}

int main(void)
{
    Point pos1(1, 3);
    std::cout << pos1;
    Point pos2(101, 303);
    std::cout << pos2;
    
    return 0;
}
```
  
</details>

- 디폴트 대입 연산자
  - 정의하지 않으면 디폴트 대입 연산자가 삽입된다.
  - 복사 생성자와 마찬가지로 멤버 대 멤버 복사(얕은 복사)를 수행한다.
  - 연산자 내에서 동적 할당을 한다면, 그리고 깊은 복사가 필요하다면 직접 정의해야 한다.
  - 객체 간의 대입연산은 대입 연산자 오버로딩을 통한 함수의 호출이라는 점에서 C언어의 구조체 변수 간 대입연산과 차이가 있다.

<details><summary>ex1</summary>

```cpp
#include <iostream>

class Monster
{
private:
    int atk, hp;
public:
    Monster& operator=(const Monster& mon)
    {
        std::cout << "operator=()" << std::endl;
        atk = mon.atk;
        hp = mon.hp;
        return *this;
    }
};

int main(void)
{
    Monster mon1;
    Monster mon2 = mon1;  // 선언과 동시에 초기화하는 경우 복사 생성자 호출
    Monster mon3;         // 객체 생성(=선언 및 초기화 수행)
    mon3 = mon2;          // 이미 생성된 객체 간 대입하는 경우 대입 연산자 호출
    return 0;
}
```

</details>

- 상속 구조에서의 대입 연산자 호출
  - 대입 연산자는 생성자가 아니다. 따라서 유도 클래스의 생성자에는 아무런 명시를 하지 않아도 기초 클래스의 생성자가 호출되지만, 유도 클래스의 대입 연산자에는 아무런 명시를 하지 않으면, 기초 클래스의 대입 연산자가 호출되지 않는다.

<details><summary>ex1</summary>

```cpp
#include <iostream>

class First
{
private:
    int num1, num2;
public:
    First(int n1 = 0, int n2 = 0) : num1(n1), num2(n2) { }
    void ShowData() { std::cout << num1 << ", " << num2 << std::endl; }

    First& operator=(const First& ref)
    {
        std::cout << "First& operator=()" << std::endl;
        num1 = ref.num1;
        num2 = ref.num2;
        return *this;
    }
};

class Second : public First
{
private: 
    int num3, num4;
public:
    Second(int n1 = 0, int n2 = 0, int n3 = 0, int n4 = 0) : First(n1, n2), num3(n3), num4(n4) { }
    void ShowData()
    {
        First::ShowData();
        std::cout << num3 << ", " << num4 << std::endl;
    }

    // 유도 클래스의 대입 연산자를 정의하지 않으면 디폴트 대입 연산자가 호출되며 디폴트 대입 연산자는 기초 클래스의 대입 연산자까지 호출한다.
    Second& operator=(const Second& ref)
    {
        // 대입 연산자를 오버로드할 때 기초 클래스의 대입 연산자도 명시적으로 호출해야 한다.
        std::cout << "Second& operator=()" << std::endl;
        First::operator=(ref);
        num3 = ref.num3;
        num4 = ref.num4;
        return *this;
    }
};


int main(void)
{
    Second ssrc(111, 222, 333, 444);
    Second scpy(0, 0, 0, 0);
    scpy = ssrc;
    scpy.ShowData();
    return 0;
}
```

</details>

## Reference

- 윤성우, <열혈 C++ 프로그래밍>
