# tic-tac-toe

Tic Tac Toe online, using C#, Docker, and a MariaDB database.

## Useful links

- [Installation guide](installation.md)
- [User documentation](UserDocumentation.md)

## Implementation

3 things are needed to make this run:

1. Database container, that, well, databases.
2. Websocket, written here in C#, that transmits messages between everyone.
3. Local apps, that act as the UI, and connect to both the WS and the DB.

## Communications

The websocket requires some data, most notably who you are, before connecting you with anyone. 
So, you'll need to send an identify message.

```json
{
    "Type": "Identify",
    "Name": "Name",
}
```

For any data to actually be stored you'll still need to use the database.

For moves, data is as follows:

````json
{
    "Type": "Move",
    "Payload": "00000"
}
```

Where the payload integer represents the move, in base 10, which will be then converted to base 3, and be used to represent the board.

For anything else, I haven't decided yet.
