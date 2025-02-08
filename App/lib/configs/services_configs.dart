import 'package:dio/dio.dart';
import 'package:get_it/get_it.dart';
import 'package:app/configs/env.dart';
import 'package:app/features/cross/create_pending_user_register/create_pending_user_register_client.dart';

final getIt = GetIt.instance;

void addServicesConfigs() {
  getIt.registerLazySingleton(() => CreatePendingUserRegisterClient());
}

// ---------------------------------------------------------------------------//

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
    // GET FROM SECURE STORAGE
    options.headers['Authorization'] = 'Bearer my_token';
    return super.onRequest(options, handler);
  }
}
