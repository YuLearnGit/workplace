var mysql=require('mysql');

/**
 * 用于配置规则时的数据库操作
 * Author：于仁飞
 * @param {*sql语句} sqlStr 
 */

function dbOperation(sqlStr){
    var connect = mysql.createConnection({ 
        host:'localhost',
        user: 'root', 
        password: '123456',
        database:'firewallclient' 
      }); 
      connect.connect();
      connect.query(
        sqlStr, 
        function(err, results) { 
          if (err) { 
              console.log(err.message);
            throw err; 
          } 
             if(results){
                for(var i = 0; i < results.length; i++)
                { 
                    console.log("%s\t",results[i].Manufacturers);
                }
            }   
        } 
      );
      connect.end();    
}exports.dbOperation=dbOperation;
