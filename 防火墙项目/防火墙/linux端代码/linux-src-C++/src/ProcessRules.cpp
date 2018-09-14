#include <sys/types.h>
#include <stdio.h>
#include <unistd.h>
#include <string.h>
#include <cppconn/exception.h>

#include <algorithm>

#include "ProcessRules.h"

#define DEBUG

using namespace std;
using namespace sql;

shared_ptr<ProcessRules> ProcessRules::self_ptr = nullptr;

ProcessRules::ProcessRules() {
  try {
      driver = mysql::get_mysql_driver_instance();
  } catch (SQLException&e) {
    //  cout << "get driver-instance failed" << endl;
  }
}

shared_ptr<ProcessRules> ProcessRules::getInstance() {
  if(self_ptr == nullptr)
    self_ptr = shared_ptr<ProcessRules>(new ProcessRules);
  return self_ptr;
}

bool ProcessRules::connectMysql() {
  try {
      con = driver->connect(host, user, passwd);
      state = con->createStatement();
      //state->execute("use firewall");
      return true;
  } catch (SQLException&e) {
    //  cout << "get connection failed" << endl;
      return false;
  }
}

void ProcessRules::closeMysql() {
  delete state;
  delete con;
  state = nullptr;
  con = nullptr;
}

bool ProcessRules::loadRules() {
  for(const auto &table : tables) {
    ResultSet *result = state->executeQuery("select rules from " + table);
    while(result->next())
      table_rules[table].push_back(result->getString("rules"));
    delete result;
  }

  return true;
}

bool ProcessRules::loadTableRules(const string &table) {
    ResultSet *result = state->executeQuery("select rules from " + table);
    while(result->next())
       table_rules[table].push_back(result->getString("rules"));

    delete result;
    return true;
}

bool ProcessRules::addRule(const std::string &rule, const std::string &table) {
  table_rules[table].push_back(rule);

#ifdef DEBUG
  cout << "-------add rule-----------" << endl;
  for(auto const &item : table_rules[table])
    cout << "table "+table+" rules are : " << item << endl;
#endif

  string QUERY = "INSERT into " + table + " (rules) VALUES ('" + rule + "')";
  if(state->executeUpdate(QUERY))
    return true;
  return false;
}

bool ProcessRules::deleteRule(const std::string &rule, const std::string &table) {

#ifdef DEBUG
  cout << "-------delete rule-----------" << endl;
  for(auto const &item : table_rules[table])
    cout << "table "+table+" rules are : " << item << endl;
  cout << "deleted rule is: " << rule << endl;
#endif

  vector<string>::iterator iter = find(table_rules[table].begin(), table_rules[table].end(), rule);
  if(iter != table_rules[table].end()) {
    table_rules[table].erase(iter);

    string QUERY = "delete from " + table + " where rules= ('" + rule + "')";
    if(state->executeUpdate(QUERY))
       return true;
    return false;
  }
  else
    return false;
}

bool ProcessRules::updateRule(const std::string &rule, const std::string &table) {

#ifdef DEBUG
  cout << "-------update rule-----------" << endl;
  for(auto const &item : table_rules[table])
    cout << "table "+table+" rules are : " << item << endl;
#endif

  size_t pos = rule.find("#");
  string new_rule = rule.substr(0, pos);
  string old_rule_flag = rule.substr(pos+1, string::npos);

#ifdef DEBUG
  cout << "new rule is : " << new_rule << endl;
  cout << "old-rule-flag is : " << old_rule_flag << endl;
  cout << "rule-table is : " << table << endl;
#endif

  string QUERY = "update " + table + " set rules='" + new_rule + "' where protocol='" + old_rule_flag + "'";

#ifdef DEBUG
  cout << "update query is : " << QUERY << endl;
#endif

  if(state->executeUpdate(QUERY)) {
    table_rules[table].clear();
    loadTableRules(table);
    return true;
  }
  else
    return false;
}

bool ProcessRules::executeRule(const std::string &rule) {
  int status = system(rule.c_str());
  if(!WIFSIGNALED(status)) {
    cout << "execute success" << endl;
    return true;
  }
  return false;
}

map<std::string, std::vector<string>> &ProcessRules::getTable_rules() {
  return table_rules;
}

bool ProcessRules::executeAllRule() {
    bool status = true;
    for(const auto &table : tables)
        for(const auto &rule : table_rules[table])
            status = status && executeRule(rule);
//system("iptables -A FORWARD -j DROP");
    return status;
}
