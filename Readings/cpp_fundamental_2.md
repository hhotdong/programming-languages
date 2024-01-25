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



## Reference

- 윤성우, <열혈 C++ 프로그래밍>
