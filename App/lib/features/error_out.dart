import 'dart:convert';

class ErrorOut {
  String code;
  String message;

  ErrorOut({
    required this.code,
    required this.message,
  });

  factory ErrorOut.fromJson(Map<String, dynamic> json) => ErrorOut(
        code: json["code"],
        message: json["message"],
      );

  Map<String, dynamic> toJson() => {
        "code": code,
        "message": message,
      };
}

ErrorOut errorOutFromJson(String str) => ErrorOut.fromJson(json.decode(str));

String errorOutToJson(ErrorOut data) => json.encode(data.toJson());
