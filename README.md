# FeedsCather
A multiuser web application for parsing and merging the news from different sources into a single news feed.
Application consists of the backend as web API (ASP.NET Core) and frontend as a SPA (Angular 4).

## Backend
### Environment
Ubuntu 16.04, MySQL v5.7.19, .NET Core v1.0.4, Redis-server v3.0.6, Nginx v1.10.3 (Ubuntu), Node.js v7.8.0, npm v4.2.0.

#### Install .NET Core
```
sudo sh -c 'echo "deb [arch=amd64] https://apt-mo.trafficmanager.net/repos/dotnet-release/ xenial main" > /etc/apt/sources.list.d/dotnetdev.list'
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 417A0893
sudo apt-get update
sudo apt-get install dotnet-dev-1.0.4
```
#### Install MySQL
```
sudo apt-get update
sudo apt-get install mysql-server
sudo mysql_secure_installation
root password: <password>
systemctl status mysql.service
sudo systemctl mysql start
```
#### Configure MySQL server default options

Paste these lines in the end of the ```sudo nano /etc/mysql/my.conf``` file:
```
[mysqld]
general_log=on
general_log_file=/var/log/mysql/general.log
default-time-zone = '+00:00'
character-set-server=utf8
collation-server=utf8_general_ci
```
In the Terminal enter:
```
mysql -u root -p
```
And run the db creation script:
```
CREATE DATABASE news_parser_db character set UTF8 collate utf8_general_ci;
GRANT ALL PRIVILEGES ON news_parser_db.* TO 'root'@'xx.xx.xx.xx' WITH GRANT OPTION;
quit
```
#### Install Nginx

```
sudo apt-get install nginx
sudo service nginx start
```
#### Install Redis

```
sudo apt install redis-server
```

#### Install Node.js and npm

```
sudo apt-get install node
sudo apt install npm
```

#### Install apiDoc

```
sudo apt-get install python-software-properties
curl -sL https://deb.nodesource.com/setup_6.x | sudo -E bash 
sudo npm insall apidoc -g
```

#### SMTP
For development purposes install maildev and run it:
```
npm install -g maildev
maildev
```
On the step of the application configuration add the maildev settings to the ```.env``` file in the SMTP section.

### Application deployment

#### Clone the project

Clone the project to the ```/var/www/newsparser```:
```
git clone https://github.com/annapogorelova/newsparser.git
```
Create ```newsparser.production.env```, ```newsparser.test.env```. The ```.env``` files can be placed anywhere on the machine - just make sure that they are not in the ```/dist``` folder. Create ```appsettings.production.json```, ```appsettings.test.json``` in the root of ```newsparser.web``` project. Fill in these settings files with the real settings corresponding to your environment by following the examples files (```appsettings.example.json``` and ```.env.example```).

#### Configure Nginx
Add a ```newsparser.web``` config file to ```/var/etc/nginx/sites-available``` and fill it with the default config that can be found in ```/config/webserver/nginx``` project directory.
Navigate to the ```/var/etc/nginx/sites-enabled``` and create a symlink to the ```newsparser.web``` config by running:
```
sudo ln -s ../sites-available/newsparser.web
```
Restart Nginx
```
sudo systemctl restart nginx
```
#### Create a systemd service file
Create a ```newsparser.web.service``` and ```newsparser.scheduler.service``` configuration file in ```/etc/systemd/system``` and populate it with the corresponding example code from the ```/config/webserver/systemd``` (make sure to specify the valid paths to the dlls).

Run
```
sudo systemctl enable newsparser.web.service
sudo systemctl start newsparser.web.service
sudo systemctl enable newsparser.scheduler.service
sudo systemctl start newsparser.scheduler.service
```

### Publish
To publish projects navigate to the ```/scripts``` directory of the root of the project and run the following commands:
```
sudo chmod +x ./publish.sh
sudo ./publish.sh <project name>
```
(the first command should only be run once)

## Frontend
### Environment

Nginx v1.10.3 (Ubuntu), Node.js v7.8.0, npm v4.2.0.

All tools needed for the frontend build should have been installed previously.

### Application deployment

Go to the ```/etc/nginx/sites-available``` and create the configuration file for the frontend project ```newsparser.frontend```. Populate it with the example code from the corresponding file in ```/config/webserver/nginx```. Then, as previously, create the symlink to it in ```/etc/nginx/sites-enabled``` and restart nginx.

In the root of frontend project create the ```.env``` file with the settings (example of code is in the ```.env.example```).
To publish frontend project go to the ```/frontend``` directory of the project and run:
```
git pull
npm run build:production
```
### DB Migrations
To add migrations or update database use newsparser.DAL.scaffolder as a startup project like this (a workaround for the current issues with EF Core):
```
dotnet ef database update --startup-project ../newsparser.DAL.scaffolder
dotnet ef migrations add MigrationName --startup-project ../newsparser.DAL.scaffolder
```
