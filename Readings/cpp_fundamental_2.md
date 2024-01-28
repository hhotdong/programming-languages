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

<details><summary>ex1</summary>

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

<details><summary>ex2</summary>

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


## Reference

- 윤성우, <열혈 C++ 프로그래밍>
