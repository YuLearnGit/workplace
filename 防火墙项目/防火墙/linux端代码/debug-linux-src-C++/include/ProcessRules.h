#ifndef PROCESSRULES_H
#define PROCESSRULES_H

#include <string>
#include <memory>
#include <vector>
#include <map>

#include <mysql_connection.h>
#include <mysql_driver.h>
#include <cppconn/resultset.h>
#include <cppconn/statement.h>
//#include "Singleton.h"

class ProcessRules {
 public:
  static std::shared_ptr<ProcessRules> getInstance();
  bool connectMysql();
  void closeMysql();
  bool addRule(const std::string &rule, const std::string &table);
  bool deleteRule(const std::string &rule, const std::string &table);
  bool updateRule(const std::string &rule, const std::string &table);
  bool loadRules();
  bool loadTableRules(const std::string &table);
  bool executeRule(const std::string &rule);
  bool executeAllRule();
  std::map<std::string, std::vector<std::string>> &getTable_rules();

 private:
  ProcessRules();

  static std::shared_ptr<ProcessRules> self_ptr;
  std::vector<std::string> tables = {"APC", "CNC", "DPI", "STD"};
  std::map<std::string, std::vector<std::string>> table_rules = { {"DPI", std::vector<std::string>{}},
                                                                  {"STD", std::vector<std::string>{}},
                                                                  {"CNC", std::vector<std::string>{}},
                                                                  {"APC", std::vector<std::string>{}} };

  sql::mysql::MySQL_Driver *driver = nullptr;
  sql::Connection *con = nullptr;
  sql::Statement *state = nullptr;
  std::string host = "tcp://localhost:3306/firewallrules";
  std::string user = "root";
  std::string passwd = "123";
};

#endif // PROCESSRULES_H
