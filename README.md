# Role Based Access Control (RBAC) using Jwt token and ASP.NET Core Web API

Implement RBAC using Jwt token and ASP.NET Core Web API for learning. 

#### Features : 
- Jwt token configuration and generator
- Role base authorization
- Custom Policy based authorization
- Environment wise Docker support (test, stage, prod and dev)

#### How to test : 
##### Running the Web API from Docker : 
- Clone the repo. 
- Open command editor (CMD, PowerShell) from clone directory and navigate to _:src\\\\jwt-authentication-server\\\\JwtAuthenticationServer_
- Run below docker command : 
```sh
docker-compose -f docker-compose.<env>.yml up
```
here, env = dev or test or stage or prod.
##### Test RBAC API :
1. From [Postman](https://www.postman.com/), create a POST request with below body parameters : 
```sh
{
"username":"manager",
"password":"manager"
}
```
2. Hit the url : http://localhost:5000/api/auth/login. You will receive a Jwt token in response.
3. Create a new GET request. Set _Authorization Header = Bearer Token_ to that token. Hit either of the API. 
```sh
http://localhost:5000/api/inventory/salaries 
http://localhost:5000/api/inventory/stock
```
Since your role is Manager, you will receive data.
4. Repeat Step#1 with below : 
```sh
{
"username":"sales",
"password":"sales"
}
```
5. Repeat Step#3. There will be an _401 = Un-authorized_ for http://localhost:5000/api/inventory/salaries 

#### Reference : 
- [Validate Jwt token](https://jwt.io/)
