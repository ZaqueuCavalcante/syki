import 'dart:convert';

class LoginIn {
  String email;
  String password;

  LoginIn({
    required this.email,
    required this.password,
  });

  factory LoginIn.fromJson(Map<String, dynamic> json) => LoginIn(
        email: json["email"],
        password: json["password"],
      );

  String toJson() => jsonEncode({
        "email": email,
        "password": password,
      });
}

LoginIn loginInFromJson(String str) => LoginIn.fromJson(json.decode(str));

String loginInToJson(LoginIn data) => json.encode(data.toJson());
