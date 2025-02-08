import 'dart:convert';

class CreatePendingUserRegisterIn {
  String email;

  CreatePendingUserRegisterIn({
    required this.email,
  });

  factory CreatePendingUserRegisterIn.fromJson(Map<String, dynamic> json) =>
      CreatePendingUserRegisterIn(
        email: json["email"],
      );

  String toJson() => jsonEncode({
        "email": email,
      });
}

CreatePendingUserRegisterIn createPendingUserRegisterInFromJson(String str) =>
    CreatePendingUserRegisterIn.fromJson(json.decode(str));

String createPendingUserRegisterInToJson(CreatePendingUserRegisterIn data) =>
    json.encode(data.toJson());
