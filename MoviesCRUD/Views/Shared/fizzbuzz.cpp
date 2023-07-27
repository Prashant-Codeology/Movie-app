#include <iostream>

void fizzBuzz(int n) {
    for (int i = 1; i <= n; ++i) {
        if (i % 3 == 0 && i % 5 == 0) {
            std::cout <<i<< "FizzBuzz \n";
        } else if (i % 3 == 0) {
            std::cout <<i<< "Fizz \n";
        } else if (i % 5 == 0) {
            std::cout <<i<<"Buzz \n";
        } else {
            std::cout << i <<"\n";
        }
    }
    std::cout << std::endl;
}

int main() {
    int n = 20;
    fizzBuzz(n);
    return 0;
}