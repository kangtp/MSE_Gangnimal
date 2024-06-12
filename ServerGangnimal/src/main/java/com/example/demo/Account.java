package com.example.demo;

import jakarta.persistence.*;

@Entity
public class Account {
	
	@Id
	@GeneratedValue(strategy=GenerationType.IDENTITY)
	@Column(name="account_id")
	private Long id;
	
	private String nickName;
	private String password;
	
	private int win;
	private int lose;

	public Account() {
		
	}

	public Account(String nickName, String password) {
		super();
		this.nickName = nickName;
		this.password = password;
	}

	public Long getId() {
		return id;
	}

	public void setId(Long id) {
		this.id = id;
	}

	public String getNickName() {
		return nickName;
	}

	public void setNickName(String nickName) {
		this.nickName = nickName;
	}

	public String getPassword() {
		return password;
	}

	public void setPassword(String password) {
		this.password = password;
	}

	public int getWin() {
		return win;
	}

	public void setWin(int win) {
		this.win = win;
	}

	public int getLose() {
		return lose;
	}

	public void setLose(int lose) {
		this.lose = lose;
	}

	@Override
	public String toString() {
		return "Account [id=" + id + ", nickName=" + nickName + ", password=" + password + "]";
	}
	
	
}
