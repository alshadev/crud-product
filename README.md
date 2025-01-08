
# CRUD Product

crud (create, read, update and delete) of application to maintain products with user login and register.


## How to run locally

Clone the repository to your directory
```bash
  git clone https://github.com/alshadev/crud-product.git
```
there will be 3 important parts, product.api, identity.api and frontend application.

```bash
  cd crud-product\src\backend\identity\Identity.API
  dotnet run
```
```bash
  cd crud-product\src\backend\product\Product.API
  dotnet run
```
```bash
  cd crud-product\src\frontend\product-crud
  npm install
  npm start
```
After running the three projects, please open http://localhost:3000/ if you want to use the UI.

Please login with username: ``admin`` and password: ``admin123``

This project also uses Swagger as an OpenApi Document, please use Swagger directly if you only want to run the backend part (It also supports authorization using JWT Tokens obtained from the Login endpoint on Identity Service).

Product API (http://localhost:7123/swagger/index.html).
Identity API (http://localhost:7456/swagger/index.html).


## Checklists
Here are the parts that have been completed
- Authentication and Authorization.
- Implement JWT based Authentication.
- Endpoint protection using Authorization.
- Create a CRUD endpoint for a product.
- Search and Filtering by name and price.
- Data validation using Data Annotation and logical flow.
- Global Exception Handling for api response.
- Implement logging using Serilog.
- Create simple view for read, create, update and delete product using ReactJs integrated with backend.
- Implement caching to endpoint get product by id and invalidate cache when product updated by hit endpoint update product.
- Implement Unit Test and Functional Test.
- Asynchronus programming by using async and await for database processing.
- Architecture and Design Pattern using Microservices and Repository Pattern with proper layer.


## TODO or Should be Improved later
Here are some things that can be developed further over a longer period of time.
- Improve and enhance the UI (styling).
- Add feature for filter search product (only done in backend level, can be tested in Swagger).




