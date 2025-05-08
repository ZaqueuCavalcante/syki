import 'dart:convert';

class SuccessOut {
    SuccessOut();

    factory SuccessOut.fromJson(Map<String, dynamic> json) => SuccessOut();

    Map<String, dynamic> toJson() => {};
}

SuccessOut successOutFromJson(String str) =>
  SuccessOut.fromJson(json.decode(str));

String successOutToJson(SuccessOut data) =>
  json.encode(data.toJson());
