- C++ 11에서 도입된 키워드로서 객체나 함수의 반환값을 컴파일 타임에 알 수 있다는 의미를 지닌다.

- 컴파일러가 컴파일 타임에 어떠한 식의 값을 결정할 수 있다면 해당 식을 상수식(Constant expression)으로, 그리고 이러한 상수식들 중에서 값이 정수인 것을 정수 상수식(Integral constant expression)으로 표현한다.

- 정수 상수식 예시
```cpp
// 배열 선언
int arr[size];

// 템플릿 타입 인자
template <int N>
struct A {
  int operator()() { return N; }
};
A<number> a;

enum A { a = number, b, c };
```

- const 키워드와의 비교
```cpp
int a;
const int b = a;      // OK. 변수 b의 값은 컴파일 타임에 알 수는 없으나 초기화된 이후에 값의 변경은 불가능하다.
constexpr int c = a;  // Error. 컴파일 타임에 a값을 알 수 없다.
const int i = 2;      // OK. 컴파일러에 따라 런타임 또는 컴파일 타임에 초기화한다.
constexpr int j = 2;  // OK. 컴파일 타임에 초기화한다.
```

- constexpr 함수
```cpp
#include <iostream>

constexpr int Factorial(int n) {
  int total = 1;
  for (int i = 1; i <= n; i++) {
    total *= i;
  }
  return total;
}

template <int N>
struct A {
  int operator()() { return N; }
};

int main() {
  A<Factorial(10)> a;
  std::cout << a() << std::endl;

  constexpr int ten = 10;
  A<Factorial(ten)> b;  // OK

  std::cout << Factorial(num) << std::endl;  // OK. constexpr함수에 인자로 컴파일 타임 상수가 아닌 값을 전달하면 일반 함수처럼 동작함.
}
```
- constexpr 함수 몸체 구현 시 제약조건
    - goto문 사용 불가
    - try문 사용 불가(C++ 20 부터 가능하게 변경)
    - 리터럴 타입이 아닌 변수 정의 불가
    - 초기화 되지 않는 변수 정의 불가
    - 실행 중간에 constexpr이 아닌 함수 호출 불가

- constexpr 생성자
```cpp
class Vector {
 public:
  constexpr Vector(int x, int y) : x_(x), y_(y) {}

  constexpr int x() const { return x_; }
  constexpr int y() const { return y_; }

 private:
  int x_;
  int y_;
};

constexpr Vector AddVec(const Vector& v1, const Vector& v2) {
  return {v1.x() + v2.x(), v1.y() + v2.y()};
}

template <int N>
struct A {
  int operator()() { return N; }
};

int main() {
  constexpr Vector v1{1, 2};
  constexpr Vector v2{2, 3};

  A<v1.x()> a;  // v1 또는 x() 중 하나라도 constexpr이 아니라면 이 코드는 컴파일되지 않는다.
  std::cout << a() << std::endl;

  A<AddVec(v1, v2).x()> b;
  std::cout << b() << std::endl;
}
```

- if constexpr
```cpp
#include <iostream>
#include <type_traits>

template <typename T>
void show_value(T t) {
  //if (std::is_pointer<T>::value) {  // Error. 템플릿 인자로 int를 갖는 코드가 생성되는 경우, int 타입에 대해 dereference를 수행하는 아래 코드에서 컴파일 에러가 발생한다.
  // if constexpr의 조건은 반드시 bool로 타입 변환될 수 있는 컴파일 타임 상수식이어야만 한다.
  // 이때, if constexpr의 조건이 참이라면 else에 해당하는 문장은 컴파일되지 않으며, 반대로 거짓이라면 else에 해당 하는 부분만 컴파일된다.
  if constexpr (std::is_pointer<T>::value) {
    std::cout << "포인터 이다 : " << *t << std::endl;
  } else {
    std::cout << "포인터가 아니다 : " << t << std::endl;
  }
}

int main() {
  int x = 3;
  show_value(x);

  int* p = &x;
  show_value(p);
}
```

### References

- [모두의 코드](https://modoocode.com/293)
