// Answer the questions using JavaScript code–

// 1.	Pass a function as an argument to another function
function greeting(name) {
  return "Assalamualaikum " + name;
}

function user(name, callback) {
  console.log(callback(name));
}

user("Asrafujjaman", greeting);

// 2.	Use arrow functions
const divide = (a, b) => a / b;
console.log(divide(20, 5));

// 3.	Provide a default parameter value
function multiply(a, b = 1) {
  return a * b;
}
console.log(multiply(5));

// 4.	Create a function that accepts unlimited arguments
function sum(...numbers) {
  return numbers.reduce(
    (accumulator, currentValue) => accumulator + currentValue,
    0,
  );
}
console.log(sum(1, 2, 3, 4, 5));

// 5.	Use named function parameters
function createUser({ name, age, email }) {
  return {
    name,
    age,
    email,
  };
}
console.log(
  createUser({
    name: "Asrafujjaman",
    age: 27,
    email: "asrafujjamandeepu@gmail.com",
  }),
);

// 6.	Create a generator function that yields multiple values
function* count() {
  yield 1;
  yield 2;
  yield 3;
}
const counter = count();
console.log(counter.next().value);
console.log(counter.next().value);
console.log(counter.next().value);

// 7.	Implement a recursive algorithm
function factorial(n) {
  if (n === 0) return 1;
  return n * factorial(n - 1);
}
console.log(factorial(5));

// 8.	Check an object type
const object = {};
console.log(typeof object);

// 9.	Use an object literal to bundle data
let personObj = {
  firstName: "Asraf",
  lastName: "Jaman",
  age: 27,
  email: "asrafujjamandeepu@gmail.com",
  isStudent: true,
  hobbies: ["coding", "reading", "traveling"],
  address: {
    present: "Dhaka",
    permanent: "Gazipur",
  },
  fullName: function () {
    return (this.firstName + " " + this.lastName).toUpperCase();
  },
};
for (let key in personObj) {
  if (typeof personObj[key] === "function") {
    console.log(`${key}: ${personObj[key]()}`);
  } else {
    console.log(`${key}: ${personObj[key]}`);
  }
}

// 10.	Check if an object has a property
console.log("firstName" in personObj);
console.log(personObj.hasOwnProperty("age"));

// 11.	Merge the properties of two objects
const a = { x: 1 };
const b = { y: 2 };
const merged = { ...a, ...b };
console.log(merged);

// 12.	Clone an object
const original = { name: "Asraf", age: 27 };
const clone1 = { ...original };
const clone2 = original; // This creates a reference
console.log(clone1);
console.log(clone2);

// 13.	Make a deep copy of an object
const original2 = {
  age: 27,
  address: { city: "Dhaka", country: "Bangladesh" },
};
const deepCopy1 = JSON.parse(JSON.stringify(original2));
console.log(deepCopy1);
const deepCopy2 = structuredClone(original2);
console.log(deepCopy2);

// 14.	Create a Enum with symbol
const Role = {
  ADMIN: Symbol("ADMIN"),
  USER: Symbol("USER"),
};
console.log(Role.ADMIN);
console.log(Role.ADMIN === Role.ADMIN); // true
console.log(Role.ADMIN === Role.USER); // false

// 15.	Create a reusable class
class Person {
  constructor(name, age) {
    this.name = name;
    this.age = age;
  }
  greet() {
    return `Hello, my name is ${this.name} and I am ${this.age} years old.`;
  }
}

const person1 = new Person("Asraf", 27);
console.log(person1.greet());
const person2 = new Person("Jaman", 30);
console.log(person2.greet());

// 16.	Add properties to a class
class Product {
  constructor(item) {
    this.item = item;
  }
}
const product1 = new Product("Laptop");
product1.price = 50000; // Adding a new property
console.log(product1);

// 17.	Use the constructor pattern to make a custom class
function PersonalInfo(firstName, lastName) {
  this.firstName = firstName;
  this.lastName = lastName;
  this.fullName = function () {
    return this.firstName + " " + this.lastName;
  };
}
const pi1 = new PersonalInfo("Asraf", "Jaman");
console.log(pi1.fullName());

// 18.	Add static methods to a class
class MathUtilites {
  static add(a, b) {
    return a + b;
  }
}
console.log(MathUtilites.add(5, 10));

// 19.	Use a static method to create objects
class User {
  constructor(name) {
    this.name = name;
  }
  static create(name) {
    return new User(name);
  }
}
const user1 = User.create("Asraf");
console.log(user1);

// 20.	Inherit functionality from another class
class Animal {
  speak() {
    console.log("Animal Sound");
  }
}
class Dog extends Animal {
  bark() {
    console.log("Woof");
  }
}
const dog = new Dog();
dog.speak(); // Inherited method
dog.bark(); // Own method

// 21.	Create a JavaScript object using literal pattern. Create a dynamic object by using the factory pattern. Use the prototype property in the dynamic object. Implementing namespace and inheritance to the dynamic object.
// 1. Implementing a Namespace to prevent global scope pollution
var MyApp = MyApp || {};

// 2. Object Literal Pattern (Static configuration)
MyApp.Config = {
  env: "production",
  version: "2.1.0",
};

// 3. Factory/Constructor for Dynamic Objects
MyApp.Vehicle = function (brand) {
  this.brand = brand;
};

// 4. Using the Prototype property (Methods shared across instances)
MyApp.Vehicle.prototype.getDetails = function () {
  return `This is a ${this.brand}.`;
};

// 5. Implementing Inheritance
MyApp.Car = function (brand, model) {
  // Call the parent constructor (Super)
  MyApp.Vehicle.call(this, brand);
  this.model = model;
};

// Link prototypes (Inheritance)
MyApp.Car.prototype = Object.create(MyApp.Vehicle.prototype);
MyApp.Car.prototype.constructor = MyApp.Car;

// Adding a method to the Child prototype
MyApp.Car.prototype.drive = function () {
  return `Driving a ${this.brand} ${this.model}.`;
};

// --- Usage ---
const myCar = new MyApp.Car("Tesla", "Model 3");
console.log(myCar.getDetails()); // Inherited: "This is a Tesla."
console.log(myCar.drive()); // Child method: "Driving a Tesla Model 3."
