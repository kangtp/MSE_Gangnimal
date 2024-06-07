package com.example.demo;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

@RestController
public class AccountController {
	
	@Autowired
	private IAccountManager mgr;
	
	@PostMapping(value="/signup", consumes="application/json")
	public boolean signUp(@RequestBody Account account) {
		System.out.print("Create account: ");
		return mgr.signUp(account.getNickName(), account.getPassword());
	}
	
	@GetMapping(value="/accounts")
	public List<Account> findAll(){
		return mgr.findAll();
	}
	
	@PostMapping(value="/signin", consumes="application/json")
	public boolean signIn(@RequestBody Account account) {
		return mgr.signIn(account);
	}
	
	@DeleteMapping(value="/delete/all")
	public boolean deleteAll() {
		System.out.println("Delete All Accounts");
		mgr.deleteAll();
		return true;
	}
	
	@GetMapping(value="/record/update/{nickname}/{flag}")
	public boolean updateBattleRecord(@PathVariable String nickname, @PathVariable String flag) {
		if(mgr.updateBattleRecord(nickname,flag) != null) {
			System.out.println("Update Success!");
			return true;
		}
		System.out.println("Update Failed!");
		return false;
	}
	
	@GetMapping(value="/record/{nickname}")
	public Account findBattleRecord(@PathVariable String nickname) {
		return mgr.findUser(nickname);
	}
}
