import 'package:dio/dio.dart';
import 'package:get/get.dart';
import 'package:app/configs/env.dart';
import 'package:app/auth/auth_service.dart';

final dio = Dio();

void addHttpConfigs() {
  dio.options.baseUrl = Env.apiUrl;
  dio.options.connectTimeout = const Duration(seconds: 5);
  dio.options.receiveTimeout = const Duration(seconds: 5);
  dio.options.headers = <String, String>{
    'Content-Type': 'application/json; charset=UTF-8',
  };

  dio.interceptors.add(JWTInterceptor());
}

class JWTInterceptor extends InterceptorsWrapper {
  @override
  Future onRequest(
    RequestOptions options,
    RequestInterceptorHandler handler,
  ) async {
    var token = await Get.find<AuthService>().getToken();
    options.headers['Authorization'] = 'Bearer $token';
    return super.onRequest(options, handler);
  }
}
