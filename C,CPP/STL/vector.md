<details><summary>Why is it called 'vector'?</summary>
   
  > It's called a vector because Alex Stepanov, the designer of the Standard Template Library, was looking for a name to distinguish it from built-in arrays.
  > He admits now that he made a mistake, because mathematics already uses the term 'vector' for a fixed-length sequence of numbers.
  > C++11 compounds this mistake by introducing a class 'array' that behaves similarly to a mathematical vector.  
  > \- [Mark Ruzon](https://stackoverflow.com/a/758548)
  
</details>

<details><summary>code example</summary>

```cpp
// Credit: The Cherno, https://youtu.be/PocJ5jXv8No?feature=shared
#include <iostream>
#include <vector>

struct Vertex
{
    float x, y, z;
};

std::ostream& operator<<(std::ostream& stream, const Vertex& vertex)    
{
    stream << vertex.x << ", " << vertex.y << ", " << vertex.z;
    return stream;
}

void Function(const std::vector<Vertex>& vertices)
{

}

int main()
{
    std::vector<Vertex> vertices;
    vertices.push_back({ 1, 2, 3 });
    vertices.push_back({ 4, 5, 6 });
    Function(vertices);

    for (int i = 0; i < vertices.size(); i++)
        std::cout << vertices[i] << std::endl;

    vertices.erase(vertices.begin() + 1);

    for (Vertex& v : vertices)
        std::cout << v << std::endl;

    std::cin.get();
}
```

</details>

<details><summary>std::vector vs std::array</summary>

  ```cpp
  // Credit: The Cherno, https://youtu.be/Xx-NcqmveDc?feature=shared
  #include <iostream>
  #include <vector>
  
  static int s_AllocationCount = 0;
  static int s_CopyCount = 0;
  static int s_MoveCount = 0;
  
  void* operator new(size_t size)
  {
      std::cout << "Allocated " << size << " bytes\n";
      s_AllocationCount++;
      return malloc(size);
  }
  
  struct Data
  {
      int Value = 0;
  
      Data() = default;
      Data(int value) : Value(value) {}
  
      Data(const Data& other)
      {
          s_CopyCount++;
          std::cout << "Copied Data\n";
      }
  
      // To avoid copying when push_back(), implement move constructor.
      Data(const Data&& other)
      {
          s_MoveCount++;
          std::cout << "Moved Data\n";
      }
  };
  
  // Add '&' to avoid copying the vector.
  static void PrintVector(std::vector<Data>& vector)
  {
      std::cout << "Size: " << vector.size() << std::endl;
      if (vector.empty())
          return;
  
      std::cout << "Elements: { ";
      for (int i = 0; i < vector.size(); i++)
      {
          std::cout << vector[i].Value;
          if (i < vector.size() - 1)
              std::cout << ", ";
      }
      std::cout << " }\n";
  }
  
  int main()
  {
      // vector's capacity is initialized to 0.
      // Then it resizes the capacity by 150% or 200% depending on compilers.
      std::vector<Data> vector;
      
      // If possible, it's better to use std::array because it's stack allocated while std::vector is heap allocated.
      // https://www.youtube.com/watch?v=wJ1L2nSIV1s
      // std::array<Data> array; 
      
      // If you can estiamte the capacity needed, let it reserved so that vector avoids copying.
      vector.reserve(5);
  
      // This constructor works like resize() so that the number of elements becomes 5 as opposed to reserve().
      // Therefore, you can access the elements 0~4 and set the value by indexing instead of push_back().
      //std::vector<Data> vector(5);
  
      for (int i = 0; i < 5; i++)
      {
          std::cout << "capacity: " << vector.capacity() << std::endl;
          // To avoid copying Data instance, use emplace_back() instead of push_back().
          //vector.push_back(Data(i));
          vector.emplace_back(i);
      }
  
      PrintVector(vector);
      
      std::cout << s_AllocationCount << " allocations\n";
      std::cout << s_CopyCount << " copies\n";
      std::cout << s_MoveCount << " moves\n";
  
      std::cin.get();
  }
  ```
    
</details>
