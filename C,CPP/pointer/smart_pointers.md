<details><summary>Code example</summary>

  ```cpp
// Credit: The Cherno, https://youtu.be/UOB7-B2MfwA?feature=shared
#include <iostream>
#include <vector>
#include <memory>

class Entity
{
public:
    Entity()
    {
         std::cout << "Created Entity!" << std::endl;
    }

    ~Entity()
    {
         std::cout << "Destroyed Entity!" << std::endl;
    }
};

int main()
{
    {
        std::shared_ptr<Entity> e0;
        {
            //std::unique_ptr<Entity> entity(new Entity());
            std::unique_ptr<Entity> entity = std::make_unique<Entity>();  // Preferred to above because of exception safety.
            
            // Copying unique pointer is not allowed.
            //std::unique_ptr<Entity> entity2 = entity;

            // Shared pointer has to allocate the memory called control block 
            // which manages reference count. Therefore, if you're using 'new' keyword,
            // it allocates twice, i.e., one for Entity and another for control block which is inefficient.
            std::shared_ptr<Entity> sharedEntity = std::make_shared<Entity>();

            // Copying shared pointer to other shared pointer increases reference count.
            e0 = sharedEntity;

            // Copying shared pointer to weak pointer doesn't increase reference count.
            // It's useful if you don't want to take owenership of the entity, just want to store the reference to that.
            std::weak_ptr<Entity> weakEntity = sharedEntity;
        }
    }
    std::cin.get();
}
  ```

</details>
