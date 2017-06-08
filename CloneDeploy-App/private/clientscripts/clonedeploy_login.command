#!/bin/bash

function clear_and_move_down()
{
  clear
  local line=0
  while [ "$line" -le "6" ]; do
    line=$(( $line + 1 ))
    echo
  done
}

function set_curl_command()
{
  export curlCommand="curl -sSk"
  export curlEnd=" --connect-timeout 10 --stderr -"
  export curlEndRedirect=" --connect-timeout 10 --stderr - >> /tmp/clientlog.log"
}

function set_curl_auth()
{
  export curlAuth="curl -sSk -H Authorization:$(echo -n "$USER_TOKEN" | base64)"
}

function parse_json()
{
  echo `echo "$1" | jq "$2" -r`
}

function check_download()
{
  if [ "$dl_result" != "200" ]; then
    echo " ...... Could Not Download Script $1 "
    echo " ...... Response Code: $dl_result "
    cat /tmp/clientscriptdlerror
    exit 1
  fi
}

function test_server_conn()
{
  local conn_result
  conn_result=$($curlCommand ${web}Test $curlEnd)
  if [ "$conn_result" != "true" ]; then
    echo "Could Not Contact CloneDeploy Server.  Enter ${web}Test In A Web Browser. "
    exit 1
  fi
}

clear_and_move_down
web=$(cat /usr/local/bin/weburl)
export web
set_curl_command
test_server_conn

loginCount=1
echo " ** CloneDeploy.  Login To Continue Or Close Window To Cancel ** "
echo
while [ "$loginCount" -le "2" ]; do	
  echo -n "Username: "
  read username
  echo -n "Password: "
  stty -echo
  read password
  stty echo
  echo

  loginResult=$($curlCommand -F username="$(echo -n $username | base64)" -F password="$(echo -n $password | base64)" -F clientIP="$(echo -n $clientIP | base64)" -F task="$(echo -n $task | base64)" "${web}ConsoleLogin" $curlEnd)
			
  if [ "$(parse_json "$loginResult" .valid)" != "true" ]; then
    if [ "$loginCount" = "2" ]; then
      echo
      echo " ...... Incorrect Login....Exiting"
      exit 1
    else
      echo
      echo " ...... Incorrect Login"
      echo
    fi
  else
    echo
    echo " ...... Login Successful"
    echo			
    export USER_TOKEN=$(parse_json "$loginResult" .user_token)
    export user_id=$(parse_json "$loginResult" .user_id)
    break
  fi
  loginCount=$(( $loginCount + 1 ))
done

set_curl_auth

clear_and_move_down
echo " ** Downloading Core Scripts ** "
for script_name in mie_global_functions mie_task_select mie_upload mie_deploy mie_cancel mie_reporter mie_register mie_ond; do
  dl_result=$($curlAuth --data "scriptName=$script_name" ${web}DownloadCoreScripts -o /usr/local/bin/$script_name -w %{http_code} --connect-timeout 10 --stderr /tmp/clientscriptdlerror)
  check_download $scriptName
  chmod +x /usr/local/bin/$script_name
done

	
echo " ...... Complete"
echo
sleep 1
/usr/local/bin/mie_task_select

