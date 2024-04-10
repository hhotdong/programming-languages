## Singly linked list

<details><summary>Find node with item</summary>

```cpp
Node* Find(T item) {
  Node* cur = first_;
  while (cur) {
    if (cur->item == item)
      return cur;
    cur = cur->next;
  }
  return cur;  // nullptr
}
```
</details>


<details><summary>Remove node</summary>

```cpp
void Remove(Node* node) {
  assert(first_);
  
  Node* prev = first_;
  while (prev->next) {
    if (prev->next == node) {
      prev->next = node->next;
      delete node;
      return;
    }
    prev = prev->next;
  }
}
```

</details>


<details><summary>Remove last node</summary>

```cpp
void PopBack() {
  if (IsEmpty()) {
    using namespace std;
    cout << "Nothing to Pop in PopBack()" << endl;
    return;
  }
  assert(first_);

  if (first_->next == nullptr) {
    delete first_;
    first_ = nullptr;
    return;
  }

  Node* second_last = first_;
  while (second_last->next->next)
    second_last = second_last->next;

  Node* temp = second_last->next;
  second_last->next = second_last->next->next;
  delete temp;
}
```

</details>


<details><summary>Reverse</summary>

```cpp
void Reverse() {
  Node* prev = nullptr;
  Node* cur  = first_;
  while (cur) {
    Node* temp = prev;
    prev = cur;
    cur = cur->next;
    prev->next = temp;
  }
  first_ = prev;
}
```

</details>

### References

- 홍정모 연구소, <자료구조 압축코스>
