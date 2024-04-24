## 커스텀 문자열

<details><summary>ex</summary>

```cpp
class MyString
{
public:
(...)
    // '\0'로 끝나는 문자열로부터 초기화
    MyString(const char* str)
    {
        // 1.
        //size_ = 0;
        //while (str[size_] != '\0')
        //    ++size_;
        
        // 2.
        size_ = strlen(str);

        str_ = new char[size_];
        memcpy(str_, str, size_);
    }
    
    void Resize(int new_size)
    {
        if (new_size != size_)
        {
            char* new_str = new char[new_size];
            memcpy(new_str, str_, new_size < size_ ? new_size : size_);
            delete[] str_;
            str_ = new_str;
            size_ = new_size;
        }
    }
    
    MyString MyString::Insert(MyString t, int start)
    {
        assert(start >= 0);
        assert(start <= this->size_);

        // 1.
        MyString ret;
        ret.Resize(size_ + t.size_);

        for (int i = 0; i < start; ++i)
            ret.str_[i] = this->str_[i];

        for (int i = start; i < start + str.size_; ++i)
            ret.str_[i] = t.str_[i - start];

        for (int i = start + t.size_; i < size_ + t.size_; ++i)
            ret.str_[i] = this->str_[i - t.size_];

        // 2.
        //MyString ret(*this);
        //ret.Resize(ret.size_ + str.size_);
        // for (int i = ret.size_ - 1; i >= start + str.size_; --i)
        //    ret.str_[i] = ret.str_[i - str.size_];
        //for (int i = 0, j = start; i < str.size_; ++i, ++j)
        //    ret.str_[j] = str.str_[i];

        return ret;
    }
    
    int MyString::Find(MyString pat)
    {
        for (int i = 0; i <= size_ - pat.size_; ++i)
        {
            int j = i, k = 0;
            while (k < pat.Length() && str_[j] == pat.str_[k])
            {
                ++j; ++k;
            }

            if (k == pat.Length())
                return i;
        }
        return -1;
    }
(...)
private:
    char* str_ = nullptr;  // 마지막에 '\0' 없음
    int size_ = 0;         // 글자 수
}
```

</details>

## Sparse polynomial

<details><summary>ex</summary>

```cpp
SparsePolynomial SparsePolynomial::Add(const SparsePolynomial& poly)
{
    int i = 0, j = 0;
    SparsePolynomial tmp;
    while (i < this->num_terms_ && j < poly.num_terms_)
    {
        if (this->terms_[i].exp == poly.terms_[j].exp)
        {
            tmp.NewTerm(this->terms_[i].coeff + poly.terms_[j].coeff, this->terms_[i].exp);
            ++i;
            ++j;
            continue;
        }
        
        if (this->terms_[i].exp < poly.terms_[j].exp)
        {
            tmp.NewTerm(this->terms_[i].coeff, this->terms_[i].exp);
            ++i;
            continue;
        }

        if (this->terms_[i].exp > poly.terms_[j].exp)
        {
            tmp.NewTerm(poly.terms_[j].coeff, poly.terms_[j].exp);
            ++j;
            continue;
        }
    }

    while (i < this->num_terms_)
    {
        tmp.NewTerm(this->terms_[i].coeff, this->terms_[i].exp);
        ++i;
    }
    
    while (j < poly.num_terms_)
    {
        tmp.NewTerm(poly.terms_[j].coeff, poly.terms_[j].exp);
        ++j;
    }

    return tmp;
}
```

</details>

- 배열 기반의 Stack에서 Pop() 함수를 호출하는 경우 인덱스만 감소하는 방식으로 구현할 수 있다.

<details><summary>ex</summary>

```cpp
void Pop()
{
    top_--;
    // 필요한 경우 소멸자를 수동으로 호출.
    // stack_[top_--].~T();
}
```
</details>

# Reference

- 홍정모 연구소, <자료구조 압축코스>
