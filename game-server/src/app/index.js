var app = require("express")();
var server = require("http").Server(app);
var io = require("socket.io")(server);

server.listen(3000);

var enemies = [];
var playerSpawnPoints= [];
var clients = [];

app.get("/", function(req, res){
    res.send("this is working");
});

io.on("connection", function(socket){
    
    var currentPlayer = {};
    currentPlayer.name = "unknown";

    socket.on("player_connect", function(){
        console.log("# " + currentPlayer.name + " recived: player_connect");
        for(var i = 0; i < clients.length; i++){
            var playerConected = {
                name: clients[i].name,
                position: clients[i].position,
                rotation: clients[i].rotation,
                health: clients[i].health
            };
            socket.emit("other_player_connected", playerConected);
            console.log("# " + currentPlayer.name + " emit: other_player_conected" + JSON.stringify(playerConected));
        }
    });

    socket.on("play", function(data){
        console.log("# " + currentPlayer.name+" recived: play: "+ JSON.stringify(data));
        if(clients.length === 0){
            numberOfEnemies = data.enemySpawnPoints.length;
            enemies = [];
            data.enemySpawnPoints.forEach(function(enemySpawnPoint){
                var enemy = {
                    name: guid(),
                    position: enemySpawnPoint.position,
                    rotation: enemySpawnPoint.rotation,
                    health: 100
                }
                enemies.push(enemy);
            });
            playerSpawnPoints = [];
            data.playerSpawnPoints.forEach(function(_playerSpawnPoint){
                var playerSpawnPoint ={
                    position: _playerSpawnPoint.position,
                    rotation: _playerSpawnPoint.rotation
                }
                playerSpawnPoints.push(playerSpawnPoint);
            });
        }
        var enemiesResponse = {
            enemies: enemies
        };
        console.log("# " + currentPlayer.name+" emit: enemies: " + JSON.stringify(enemiesResponse));
        socket.emit("enemies", enemiesResponse);
        var randomSpawnPoint = playerSpawnPoints[Math.floor(Math.random() * playerSpawnPoints.length)];
        currentPlayer = {
            name: data.name,
            position: randomSpawnPoint.position,
            rotation: randomSpawnPoint.rotation,
            health: 100
        };
        clients.push(currentPlayer);
        console.log("# " + currentPlayer.name+" emit: play: " +JSON.stringify(currentPlayer));
        socket.emit("play", currentPlayer);
        socket.broadcast.emit("other_player_connected", currentPlayer);
    });

    socket.on("player_move", function(data){
        console.log("@ " + "received: player_move " + JSON.stringify(data));
        currentPlayer.position = data.position;
        socket.broadcast.emit("player_move", currentPlayer);
    });

    socket.on("player_turn", function(data){
        console.log("@ " + "received: player_turn " + JSON.stringify(data));
        currentPlayer.rotation = data.rotation;
        socket.broadcast.emit("player_turn", currentPlayer);
    });

    socket.on("player_shoot", function(){
        console.log("# " + currentPlayer.name +" recv: player_shoot");
        var data = {
            name: currentPlayer.name
        };
        console.log("# " + currentPlayer.name + " broadcasting: player_shoot: " + JSON.stringify(data));
        socket.emit("player_shoot", data);
        socket.broadcast.emit("player_shoot", data);
    });

    socket.on("health", function(data){
        console.log("@ " + currentPlayer.name + "received: health: " + JSON.stringify(data));
        if(data.from === currentPlayer.name){
            if(!data.isEnemy){
                clients = clients.map(function(client, index){
                    if(client.name === data.name){
                        indexDamaged = index;
                        client.health -= data.healthChange;
                    }
                    return client;
                });
            }else{
                enemies = enemies.map(function(enemy, index){
                    if(enemy.name === data.name){
                        indexDamaged = index;
                        enemy.health -= data.healthChange;
                    }
                    return enemy;
                });
            }

            var response = {
                name: (!data.isEnemy) ? clients[indexDamaged].name : enemies[indexDamaged].name,
                health: (!data.isEnemy) ? clients[indexDamaged].health : enemies[indexDamaged].health
            };
            console.log("# " + currentPlayer.name+ "broadcasting: health: " + JSON.stringify(response));
            socket.emit("health", response);
            socket.broadcast.emit("health", response);
        }
    })

    socket.on("disconnect", function(){
        console.log("@ " + currentPlayer.name+" received: disconnect " +currentPlayer.name);
        socket.broadcast.emit("other_player_disconnected", currentPlayer);
        console.log("# " + currentPlayer.name + " broadcasting: other_player_disconnected: " + JSON.stringify(currentPlayer));
        for(var i=0; i<clients.length;i++){
            if(clients[i].name === currentPlayer.name){
                clients.splice(i,1);
            }
        }
    });
});

console.log("=== Server running ...");

function guid(){
    function s4(){
        return Math.floor((1+Math.random()) * 0x10000).toString(16).substring(1);
    }
    return s4() + s4() + "-" + s4() + "-" + s4() + "-" + s4() + "-" + s4() + s4() + s4();
}
