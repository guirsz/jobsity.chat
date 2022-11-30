import { Component, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup
} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/AuthService';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  form = new FormGroup({
    email: new FormControl('', { nonNullable: true }),
    password: new FormControl('', { nonNullable: true }),
  });

  constructor(
    private authService: AuthService,
  ) { }


  ngOnInit(): void {
  }

  login() {
    const val = this.form.value;
    if (val.email && val.password) {
      this.authService.login(val.email, val.password);
    }
  }

}
