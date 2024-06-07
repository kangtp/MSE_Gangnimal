package com.example.demo;

import java.util.List;

public interface IAccountManager {
	
	boolean signUp(String nickName, String password); 
	boolean signIn(Account account);
	Account findUser(String nickName);
	List<Account> findAll();
	boolean deleteAll();
	Account updateBattleRecord(String nickName, String flag);
}
