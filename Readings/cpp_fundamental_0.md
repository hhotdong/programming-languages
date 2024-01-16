# 함수 오버로딩

- C++에서는 매개변수의 선언형태가 다르면 동일한 이름의 함수정의를 허용한다. 이는 함수호출 시 전달되는 인자를 통해서 호출하고자 하는 함수의 구분이 가능하기 때문이다. 즉, 호출할 함수를 찾을 때 함수명만 이용하는 C와 달리 C++은 함수명과 매개변수의 선언을 동시에 이용한다.

- 오버로딩 조건:  매개변수의 자료형 또는 개수가 달라야 한다. 

# 매개변수 디폴트 값

- 매개변수에 디폴트 값이 설정되어 있으면, 선언된 매개변수의 수보다 적은 수의 인자전달이 가능하다. 그리고 전달되는 인자는 왼쪽에서부터 채워져 나가고, 부족분은 디폴트 값으로 채워진다.
- 디폴트 값은 함수의 선언 부분에만 표현하면 된다.

<details><summary>ex</summary>

```cpp
int Adder(int num1=1, int num2=2);

(...)

int Adder(int num1, int num2)
{   
    return num1+num2;
}
```

</details>

- 반드시 오른쪽 매개변수의 디폴트 값부터 채워져야 한다. 그 이유는 함수에 전달되는 인자가 왼쪽에서부터 오른쪽으로 채워지기 때문이다.

<details><summary>ex</summary>

```cpp
int YourFunc (int num1   , int num2   , int num3=30) {...} (O)
int YourFunc (int num1   , int num2=20, int num3=30) {...} (O)
int YourFunc (int num1=10, int num2=20, int num3=30) {...} (O)
int WrongFunc(int num1=10, int num2   , int num3)    {...} (X)
int WrongFunc(int num1=10, int num2=20, int num3)    {...} (X)
```

</details>

- 아래와 같이 오버로딩된 두 함수를 선언하면 오버로딩 조건을 만족하므로 함수 컴파일은 성공한다. 그러나 매개변수 없이 함수를 호출하게 되면 두 함수 모두 호출조건을 만족하기에 컴파일 에러가 발생한다.

<details><summary>ex</summary>

```cpp
int SimpleFunc(int a=1);  // 컴파일 성공
int SimpleFunc(void);     // 컴파일 성공
int main(void)
{
    SimpleFunc();  // 컴파일 에러
}
```

</details>

# 인라인 함수

- 매크로 함수는 일반적인 함수에 비해 실행속도의 이점이 있다는 장점이 있지만 정의하기가 어렵고 복잡한 함수를 매크로화하는데 한계가 있다. 인라인 함수는 매크로 함수의 장점은 유지하고 단점을 보완한다.
- 함수의 몸체부분이 함수호출 문장을 완전히 대체했을 때 '함수가 인라인화 되었다'고 표현한다.
- 매크로를 이용한 함수의 인라인화는 전처리기에 의해서 처리되지만, 키워드 inline을 이용한 함수의 인라인화는 컴파일러에 의해서 처리가 된다. 따라서 컴파일러는 함수의 인라인화가 오히려 성능에 해가 된다고 판단할 경우, 이 키워드를 무시해버리기도 한다. 또한 컴파일러는 필요한 경우 일부 함수를 임의로 인라인 처리하기도 한다.
- 매크로 함수는 자료형에 의존적이지 않지만 인라인 함수는 템플릿을 이용하지 않는 경우 자료형에 의존적이게 된다.

<details><summary>ex</summary>

```cpp
// 매크로 함수
#define SQUARE(x) ((x)*(x))

// 인라인 함수(템플릿 미사용)
inline int SQUARE(int x) { return x*x; }

// 인라인 함수(템플릿 사용)
template <typename T>
inline T SQUARE(T x) { return x*x; }
```

</details>

# 이름공간

- 범위 지정 연산자(scope resolution operator) '::'를 사용해서 이름공간에 접근한다.
- 범위 지정 연산자는 전역 변수를 명시적으로 표현할 때도 사용할 수 있다.
- 이름공간은 둘 이상의 역역으로 나뉘어서 선언할 수도 있다.
- 동일한 이름공간에 정의된 함수를 호출할 때에는 이름공간을 명시할 필요가 없다.

<details><summary>ex</summary>

```cpp
namespace Test1
{
    void Foo(void);
}

namespace Test1
{
    void Bar(void);
}

namespace Test2
{
    void Koo(void);
}

void Test1::Foo(void)
{
    Bar();
    Test2::Koo();
}
```

</details>

- using을 이용한 이름공간의 명시: 스코프 내에 존재하면 선언된 이후부터 효력 발생하고 스코프 벗어나면 효력 사라짐. 소스코드 전체에 대해 적용하고자 하면 함수 바깥에 선언해야 함.
- 이름공간이 과도하게 중첩된 경우 별칭을 활용 가능

<details><summary>ex</summary>

```cpp
namespace ABC=AAA::BBB::CCC;
```

</details>

# bool 자료형

- 참, 거짓을 표현하기 위한 1바이트 크기의 데이터
- true는 1이 아니며, false 역시 0이 아니다. 다만, 이 둘을 출력하거나 정수의 형태로 형 변환하는 경우에 각각 1과 0으로 변환되도록 정의되어 있을 뿐이다.

# 참조자

- 변수는 할당된 메모리 공간에 붙여진 이름이다. 그리고 그 이름을 통해서 해당 메모리 공간에 접근이 가능하다.
- 참조자를 이용해 할당된 메모리 공간에 둘 이상의 이름을 부여할 수 있다. 즉, 변수에 별칭을 부여하는 것이다.
- 변수와 참조자는 선언 방식에 차이가 있지만, 일단 선언되고 나면 변수와 차이가 없다. & 연산자로 주소 값을 반환 받을 수도 있고, 함수 내에서 선언된 지역적 참조자는 지역변수와 마찬가지로 함수를 빠져나가면 소멸된다.
- 참조자의 수에는 제한이 없으며, 참조자를 대상으로도 참조자를 선언할 수 있다.

<details><summary>ex</summary>

```cpp
int num1 = 1234;
int &num2 = num1;
int &num3 = num1;
int &num4 = num1;

int &num2=num1;
int &num3=num2;
int &num4=num3;
```

</details>

- 참조자는 변수에 대해서만 선언이 가능하고, 선언됨과 동시에 누군가를 참조해야만 한다. NULL로 초기화하는 것도 불가능하다.
- 미리 참조자를 선언했다가 후에 다른 누군가를 참조하는 것은 불가능하며, 참조의 대상을 바꾸는 것도 불가능하다.

<details><summary>ex</summary>

```cpp
int &ref;         (X)
int &ref = 20;    (X)
int &ref = NULL;  (X)
```

</details>

- 배열요소는 변수로 간주되어 참조자의 선언이 가능하다.

<details><summary>ex</summary>

```cpp
int arr[3] = { 1, 3, 5 };
int &ref1 = arr[0];
int &ref2 = arr[1];
int &ref3 = arr[2];
```

</details>

- 포인터 변수도 변수이기 때문에 참조자의 선언이 가능하다.

<details><summary>ex</summary>

```cpp
int num = 12;
int *ptr = &num;
int **dptr = &ptr;

int &ref = num;
int *(&pref) = ptr;
int **(&dpref) = dptr;
```

</details>

- Call-by-value: 값을 인자로 전달하는 함수의 호출방식
- Call-by-reference: 주소 값을 인자로 전달하는 함수의 호출방식. 더 정확히는 주소 값을 전달 받아서, 함수 외부에 선언된 변수에 접근하는 형태의 함수호출

- C와 달리 C++에서는 아래와 같은 코드에서 어떤 값이 호출될지 알 수 없다. 그 이유는 함수 인자가 참조자로 선언된 경우 함수 내부에서 외부에 선언된 변수의 값을 변경할 수 있기 때문이다. 따라서 함수 내에서 값의 변경이 발생하지 않는다면 참조자를 const로 선언하는 방식으로 이를 보완할 수 있다.

<details><summary>ex</summary>

```cpp
int num = 24;
HappyFunc(num);
std::cout<<num<<endl;
```

</details>

- 함수의 반환형이 참조형일 수 있다. 그리고 참조자를 반환하지만 반환형은 참조형이 아닐 수 있다.

<details><summary>ex</summary>

```cpp
// ref는 지역변수와 동일한 성격을 갖기에 함수가 반환하면 참조자 ref는 소멸되지만 참조자가 참조하는 변수는 소멸되지 않는다.
int & RefRetFuncOne(int &ref)
{
    ref++;
    return ref;
}

int RefRetFuncTwo(int &ref)
{
    ref++;
    return ref;
}

int main(void)
{
    int num1 = 1;
    int &num2 = RefRetFuncOne(num1);

    num1++;
    num2++;
    std::cout<<"num1: "<<num1<<endl;  // 4
    std::cout<<"num2: "<<num2<<endl;  // 4

    // 참조형으로 반환되지만 참조자가 아닌 일반변수를 선언해서 반환 값을 저장할 수 있다.
    int num3 = RefRetFuncOne(num1);

    num1 += 1;
    num3 += 100;
    std::cout<<"num1: "<<num1<<endl;  // 6
    std::cout<<"num3: "<<num3<<endl;  // 105

    int num4=RefRetFuncTwo(num1);

    num1 += 1;
    num4 += 100;
    std::cout<<"num1: "<<num1<<endl;  // 8
    std::cout<<"num4: "<<num4<<endl;  // 107

    return 0;
}
```

</details>

- 반환형이 참조형인 함수의 반환 값은 아래와 같이 두 가지 형태로 저장할 수 있다.

<details><summary>ex</summary>

```cpp
int num2 = RefRetFuncOne(num1);
int &num2 = RefRetFuncOne(num1);
```

</details>

- 반환형이 기본자료형으로 선언된 함수의 반환 값은 반드시 변수에 저장해야 한다. 반환 값은 상수와 다름없기 때문이다.

<details><summary>ex</summary>

```cpp
int num2 = RefRetFuncTwo(num1);   // O
int &num2 = RefRetFuncTwo(num1);  // X
```

</details>

- const 참조자와 상수화된 변수

<details><summary>ex</summary>

```cpp
const int num = 20;
// int & ref = num;     // X, 컴파일 에러
const int & ref = num;  // O
```

</details>

- const 참조자는 상수도 참조 가능하다.
- 리터럴 상수는 임시적으로 존재하는 값이며 다음 행으로 넘어가면 존재하지 않는 상수다. C++에서는 이러한 리터럴 상수를 참조하는 기능을 지원한다. 이를 위해 const 참조자를 이용해서 상수를 참조할 때 임시변수라는 것을 만들고 이 장소에 상수를 저장하고나서 참조자가 이를 참고하게끔 한다. 임시로 생성한 변수를 상수화하여 이를 참조자가 참조하게끔 하는 구조이니, 결과적으로는 상수화된 변수를 참조하는 형태가 된다.

<details><summary>ex</summary>

```cpp
const int &ref = 50;

int Adder(const int &num1, const int &num2)
{
    return num1 + num2;
}
```

</details>

- new 연산자를 이용해서 힙에 할당된 메모리 공간에도 참조자의 선언이 가능하다.

```cpp
int * ptr = new int;
int & ref = *ptr;
```
