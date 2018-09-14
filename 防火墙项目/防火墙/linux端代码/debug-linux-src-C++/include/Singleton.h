#ifndef SINGLETON_H_INCLUDED
#define SINGLETON_H_INCLUDED

#include <memory>

template <typename T>
class Singleton {
 public:
  static std::shared_ptr<T> getInstance();

 protected:
  Singleton() {};
  ~Singleton() {};

 private:
  Singleton(const Singleton& rhs) {}
  Singleton& operator = (const Singleton& rhs) {}

  static std::shared_ptr<T> self_ptr;
};

template <typename T>
std::shared_ptr<T> Singleton<T>::self_ptr = nullptr;

template <typename T>
std::shared_ptr<T> Singleton<T>::getInstance() {
    if(self_ptr == nullptr)
        self_ptr = std::shared_ptr<T>(new T());
    return self_ptr;
}

#endif // SINGLETON_H_INCLUDED
