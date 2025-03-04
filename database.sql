CREATE DATABASE IF NOT EXISTS tictactoe_online;

USE tictactoe_online;

CREATE TABLE IF NOT EXISTS users (
    id INT(11) UNSIGNED PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(25) NOT NULL,
    pwdHash VARCHAR(64) NOT NULL
);

CREATE TABLE IF NOT EXISTS messages (
    id INT(11) UNSIGNED AUTO_INCREMENT,
    senderId INT(11) UNSIGNED NOT NULL,
    receiverId INT(11) UNSIGNED NOT NULL,
    content TEXT NOT NULL,
    created TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    PRIMARY KEY (id, senderId, receiverId),
    FOREIGN KEY (senderId) REFERENCES users(id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (receiverId) REFERENCES users(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS game_states (
    id INT(11) UNSIGNED AUTO_INCREMENT,
    player1Id INT(11) UNSIGNED NOT NULL,
    player2Id INT(11) UNSIGNED NOT NULL,
    state INT(5) UNSIGNED NOT NULL,
    isDone BOOLEAN NOT NULL DEFAULT 0,

    PRIMARY KEY (id, player1Id, player2Id),
    FOREIGN KEY (player1Id) REFERENCES users(id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (player2Id) REFERENCES users(id) ON DELETE CASCADE ON UPDATE CASCADE
);

-- CREATE USER 'tictactoe_user'@'localhost' IDENTIFIED BY 'tictactoe_password';
-- GRANT SELECT, INSERT, DELETE on tictactoe_online.* TO 'tictactoe_user'@'localhost';

DROP USER IF EXISTS 'tictactoe_user'@'%';
CREATE USER 'tictactoe_user'@'%' IDENTIFIED BY 'tictactoe_password';
GRANT ALL PRIVILEGES ON tictactoe_online.* TO 'tictactoe_user'@'%' WITH GRANT OPTION;
FLUSH PRIVILEGES;