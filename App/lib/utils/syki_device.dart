import 'dart:io';

class SykiDevice {
  static Future<bool> hasInternet() async {
    try {
      final result = await InternetAddress.lookup('goole.com');
      return result.isNotEmpty && result[0].rawAddress.isNotEmpty;
    } on SocketException catch (_) {
      return false;
    }
  }

  static bool isIOS() {
    return Platform.isIOS;
  }

  static bool isAndroid() {
    return Platform.isAndroid;
  }
}
