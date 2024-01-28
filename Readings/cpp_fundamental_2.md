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

- 멤버 이니셜라이저가 성능 향상에 도움이 되는 이유

<details><summary>ex</summary>

```cpp
#include <iostream>

class AAA
{
private:
    int num;
public:
    AAA(int n = 0)      : num(n)       { std::cout << "AAA(int n = 0)"      << std::endl; }
    AAA(const AAA& ref) : num(ref.num) { std::cout << "AAA(const AAA& ref)" << std::endl; }
    AAA& operator=(const AAA& ref)
    {
        num = ref.num;
        std::cout << "operator=(const AAA& ref)" << std::endl;
        return *this;
    }
};

class BBB
{
private:
    AAA mem;
public:
    BBB(const AAA& ref) : mem(ref) { }
};

class CCC
{
private:
    AAA mem;
public:
    CCC(const AAA& ref) { mem = ref; }
};

int main(void)
{
    AAA obj1(12);
    std::cout << "****************" << std::endl;
    BBB obj2(obj1);
    std::cout << "****************" << std::endl;
    CCC obj3(obj1);
    return 0;
}
```

</details>

- 배열의 인덱스 연산자 오버로딩
  - C, C++의 기본 배열은 경계 검사를 하지 않는 특성이 있다.
<details><summary>ex</summary>

```cpp
#include <iostream>

int main(void)
{
    int arr[3] = { 1, 2, 3 };
    std::cout << arr[-1];  // 'arr의 주소 + sizeof(int) * -1'의 위치에 접근
    std::cout << arr[-2];  // 'arr의 주소 + sizeof(int) * -2'의 위치에 접근
    std::cout << arr[3];
    std::cout << arr[4];
    return 0;
}
```

</details>

- 경계 검사를 수행하는 배열 클래스
  
<details><summary>ex</summary>

```cpp
#include <iostream>

class BoundCheckIntArray
{
private:
    int * arr;
    int arrlen;

    // 배열은 저장소의 일종이고, 저장소에 저장된 데이터는 '유일성'이 보장돼야 하기 때문에 대부분의 경우 저장소의 복사는 불필요하거나 잘못된 일로 간주된다.
    // 따라서 깊은 복사가 진행되도록 클래스를 정의할 것이 아니라, 복사 생성자와 대입 연산자를 private 멤버로 둠으로써 복사와 대입을 원천적으로 막는 것이 좋은 선택이 되기도 한다.
    BoundCheckIntArray(const BoundCheckIntArray& ref) { }
    BoundCheckIntArray& operator=(const BoundCheckIntArray& ref) { }
public:
    BoundCheckIntArray(int len) : arrlen(len)
    {
        arr = new int[len];
    }

    // 반환형이 참조형이므로 배열요소의 참조값이 반환되고, 이 값을 이용해서 배열요소에 저장된 값의 참조 뿐만 아니라 변경도 가능하다.
    int& operator[](int idx)
    {
        if (idx < 0 || idx >= arrlen)
        {
            std::cout << "Array index out of bound exception" << std::endl;
            exit(1);
        }
        return arr[idx];
    }

    // const의 선언 유무도 오버로딩 조건에 해당한다. 참조값이 아닌 단순 값을 반환한다.
    int operator[](int idx) const
    {
        if (idx < 0 || idx >= arrlen)
        {
            std::cout << "Array index out of bound exception" << std::endl;
            exit(1);
        }
        return arr[idx];
    }

    int GetArrLen() const { return arrlen; }

    ~BoundCheckIntArray()
    {
        delete[] arr;
    }
};

void ShowAllData(const BoundCheckIntArray& ref)
{
    int len = ref.GetArrLen();
    for (int idx = 0; idx < len; idx++)
        std::cout << ref[idx] << std::endl;
}

int main(void)
{
    BoundCheckIntArray arr(5);
    for(int i = 0; i < 5; i++)
        arr[i] = (i+1) * 11;

    ShowAllData(arr);

    std::cout << "***************" << std::endl;

    for(int i = 0; i < 6; i++)
        std::cout << arr[i] << std::endl;
    return 0;
}
```

</details>

- 객체의 저장을 위한 배열 클래스 정의

<details><summary>ex</summary>

```cpp
#include <iostream>
#include <cstdlib>

class Point
{
private:
    int xpos, ypos;
public:
    Point(int x = 0, int y = 0) : xpos(x), ypos(y) { }
    friend std::ostream& operator<<(std::ostream& os, const Point& pos);
};

std::ostream& operator<<(std::ostream& os, const Point& pos)
{
    os << '[' << pos.xpos << ", " << pos.ypos << ']' << std::endl;
    return os;
}

typedef Point* POINT_PTR;  // 연산의 주 대상이 포인터인 경우, 별도의 자료형을 정의하는 게 좋다.

//class BoundCheckPointArray
class BoundCheckPointPtrArray
{
private:
    /*  배열에 객체를 저장하는 방식
    Point* arr;
    int arrlen;
    BoundCheckPointArray(const BoundCheckPointArray& arr) { }
    BoundCheckPointArray& operator=(const BoundCheckPointArray& arr) { }
    */
    
    // 배열에 객체의 주소를 저장하는 방식
    POINT_PTR* arr;
    int arrlen;
    BoundCheckPointPtrArray(const BoundCheckPointPtrArray& arr) { }
    BoundCheckPointPtrArray& operator=(const BoundCheckPointPtrArray& arr) { }
public:
    //BoundCheckPointArray(int len) : arrlen(len)
    BoundCheckPointPtrArray(int len) : arrlen(len)
    {
        //arr = new Point[len];  // Point클래스에 정의된 생성자에 의해 x, y값이 0으로 초기화된다.
        arr = new POINT_PTR[len];
    }
    
    //Point& operator[](int idx)
    POINT_PTR& operator[](int idx)
    {
        if (idx < 0 || idx >= arrlen)
        {
            std::cout << "Array index out of bound exception" << std::endl;
            exit(1);
        }
        return arr[idx];
    }
    
    //Point operator[](int idx) const
    POINT_PTR operator[](int idx) const
    {
        if (idx < 0 || idx >= arrlen)
        {
            std::cout << "Array index out of bound exception" << std::endl;
            exit(1);
        }
        return arr[idx];
    }
    
    int GetArrLen() const { return arrlen; }
    
    //~BoundCheckPointArray() { delete[] arr; }
    ~BoundCheckPointPtrArray() { delete[] arr; }
};

int main(void)
{
    /* 배열에 객체를 저장하는 방식
    BoundCheckPointArray arr(3);
    arr[0] = Point(3,4);  // 임시객체 생성 후 디폴트 대입 연산자를 호출하는 방식으로 배열요소를 초기화하고 있다.
    arr[1] = Point(5,6);
    arr[2] = Point(7,8);
    for (int i = 0; i < arr.GetArrLen(); i++)
        std::cout << arr[i];
    */

    // 배열에 객체의 주소를 저장하는 방식
    BoundCheckPointPtrArray arr(3);
    arr[0] = new Point(3,4);  // 객체의 주소 값을 저장하는 경우, 깊은 복사냐 얕은 복사냐 하는 문제를 신경 쓰지 않아도 된다.
    arr[1] = new Point(5,6);
    arr[2] = new Point(7,8);
    for (int i = 0; i < arr.GetArrLen(); i++)
        std::cout << *(arr[i]);
    delete arr[0];
    delete arr[1];
    delete arr[2];

    return 0;
}
```

</details>

- new 연산자의 역할
  1. 메모리 공간의 할당
  2. 생성자의 호출
  3. 할당하고자 하는 자료형에 맞게 반환된 주소 값의 형 변환

- new 연산자의 오버로딩은 위 3가지 항목 중 1번(메모리 공간의 할당)만 오버로딩할 수 있다. 나머지 두 가지 작업은 컴파일러에 의해서 진행되며 오버로딩이 불가능하다.

```cpp
// new 연산자의 오버로딩 함수는 반드시 void* 타입을 반환해야 하고, 매개변수 타입은 size_t이어야 한다.
// 컴파일러에 의해서 필요한 메모리 공간의 크기가 바이트 단위로 계산되어 인자로 전달된다.
// 아래 구현된 함수 몸체는 new 연산자를 오버로딩하지 않아도 수행되는 작업이기에 오버로딩을 하고자 한다면 그 이상의 일을 처리할 목적을 갖고 있어야 한다.
void* operator new(size_t size)
{
    void* adr = new char[size];  // malloc() 함수로 대체할 수 있다.
    return adr;
}

// new 연산자를 이용한 배열 할당 시 호출된다.
void* operator new[](size_t size) { }
```

- delete 연산자 오버로딩

```cpp
// 아래 문장을 통해 객체의 소멸을 명령하면, 컴파일러는 먼저 ptr이 가리키는 객체의 소멸자를 호출한다. 이어서 아래의 형태로 정의된 함수에 ptr에 저장된 주소 값을 전달한다.
delete ptr;

// 소멸자는 오버로딩된 delete 함수가 호출되기 전에 호출되므로 오버로딩된 함수에서는 메모리 공간의 소멸을 책임져야 한다.
void operator delete(void* adr)
{
    delete[] adr;  // free() 함수로 대체할 수 있다.
}

// delete 연산자를 이용한 배열 소멸 시 호출된다.
void operator delete[](void* adr) { }
```

- operator new, operator delete 함수는 멤버함수 형태로 선인을 해도 static 함수로 간주되므로 객체 생성의 과정에서 호출이 가능한 것이다.

<details><summary>ex</summary>

```cpp
#include <iostream>

class Point
{
private:
    int xpos, ypos;
public:
    Point(int x = 0, int y = 0) : xpos(x), ypos(y) { }
    friend std::ostream& operator<<(std::ostream& os, const Point& pos);

    ~Point()
    {
        std::cout << "Destruct : " << xpos << ", " << ypos << std::endl;
    }

    void* operator new(size_t size)
    {
        std::cout << "operator new : " << size << std::endl;
        void* adr = new char[size];
        return adr;
    }

    void* operator new[](size_t size)
    {
        std::cout << "operator new [] : " << size << std::endl;
        void* adr = new char[size];
        return adr;
    }

    void operator delete(void* adr)
    {
        std::cout << "operator delete ()" << std::endl;
        delete[] (char*)adr;
    }

    void operator delete[](void* adr)
    {
        std::cout << "operator delete[] ()" << std::endl;
        delete[] (char*)adr;
    }
};

std::ostream& operator<<(std::ostream& os, const Point& pos)
{
    os << '[' << pos.xpos << ", " << pos.ypos << ']' << std::endl;
    return os;
}

int main(void)
{
    Point* ptr = new Point(3,4);
    Point* arr = new Point[3];
    delete ptr;
    delete[] arr;

    return 0;
}
```

</details>

## Reference

- 윤성우, <열혈 C++ 프로그래밍>
