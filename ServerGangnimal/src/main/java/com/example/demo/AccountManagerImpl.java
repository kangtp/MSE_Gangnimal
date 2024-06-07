package com.example.demo;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import jakarta.transaction.Transactional;

@Service
public class AccountManagerImpl implements IAccountManager{
	
	@Autowired
	private AccountRepository repo;

	@Override
	public boolean signUp(String nickName, String password) {
		if(findUser(nickName) == null) {
			repo.save(new Account(nickName, password));
			return true;
		}
		return false;
	}

	@Override
	public boolean signIn(Account account) {
		Account a = repo.findByNickName(account.getNickName());
		if(a != null) {
			if(a.getPassword().equals(account.getPassword())) {
				return true;
			}
		}
		return false;
	}

	@Override
	public Account findUser(String nickName) {
		return repo.findByNickName(nickName);
	}

	@Override
	public List<Account> findAll() {
		return repo.findAll();
	}

	@Override
	public boolean deleteAll() {
		repo.deleteAll();
		return true;
	}

	@Transactional
	@Override
	public Account updateBattleRecord(String nickName, String flag) {
		Account a = repo.findByNickName(nickName);
		System.out.println(nickName + "/" + flag + "/" + a);
		if(a != null) {
			if(flag.equals("win")) {
				System.out.println("flag == win");
				a.setWin(a.getWin() + 1);
			}
			else if(flag.equals("lose")){
				System.out.println("flag == lose");
				a.setLose(a.getLose() + 1);
			}
			else {
				System.out.println(flag);
				return null;
			}
			return a;
		}
		return null;
	}

	
}
