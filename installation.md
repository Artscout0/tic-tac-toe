# Installation Documentation

## requirements

The server require docker and docker compose.

By default both the server and the app are run on the same machine.

## default version

To run said default, you need to run `docker compose up` in the same directory as the source code (specifically the docker-compose.yml file).
This will automatically set up the database and websocket server. You then need to launch the app executable, and everything should work by default.

If you for some reason don't have the executable, it can be built using visual studio with the code in tic-tac-toe.

## advanced version

To run the server on a different machine from the app, you transfer the source code to the server, and run the same command (`docker compose up`).

The executable will need to know where the server is. When launched it creates a hostname.txt file, which you can edit with the IP of the server, and the app will try to connect to that IP.
