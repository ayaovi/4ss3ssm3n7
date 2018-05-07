# Introduction

The project is made of an api and a front end:

* api (can be found in the src/ directory)
* ui (can be found in the ui/ directory)

# Execution

It is recommanded to have the api running before starting the  ui.

# Set up

## api

The api is backed by a Msql database. To setup, please modify the database connection string to specify the appropriate "user" and "password" (e.g. server=localhost;database=sales;user=user;password=password). Thereafter:

* Start the api to ensure database and its object are created.
* Run the sql script to populate the "materials", "clients", "items", "orders", "orderlines" tables.

## ui

With the api running, open the ui/orders.html in a browser. At first, the list of orders in the system are displayed. The following actions can be performed on each order:

* Edit: make changes to the order, by either changing some of the existing order lines or by adding new ones. 
* Delete: removes the order from the system.

As for adding a new order, one needs to select the appropriate client id and item ids.