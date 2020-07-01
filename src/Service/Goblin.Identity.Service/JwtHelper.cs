using System;
using System.Text;
using Goblin.Identity.Core;
using LitJWT;
using LitJWT.Algorithms;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Goblin.Identity.Service
{
 public class JwtHelper
    {
        private static JwtEncoder _jwtEncoder;

        private static JwtDecoder _jwtDecoder;

        protected static JwtEncoder JwtEncoder
        {
            get
            {
                if (_jwtEncoder != null)
                {
                    return _jwtEncoder;
                }

                var keyBytes = Encoding.UTF8.GetBytes(SystemSetting.Current.TokenEncryptKey);

                _jwtEncoder = new JwtEncoder(new HS256Algorithm(keyBytes));

                return _jwtEncoder;
            }
        }

        protected static JwtDecoder JwtDecoder
        {
            get
            {
                if (_jwtDecoder != null)
                {
                    return _jwtDecoder;
                }

                var keyBytes = Encoding.UTF8.GetBytes(SystemSetting.Current.TokenEncryptKey);

                _jwtDecoder = new JwtDecoder(new HS256Algorithm(keyBytes));

                return _jwtDecoder;
            }
        }

        /// <summary>
        ///     Generate Token with Payload
        /// </summary>
        /// <param name="payload"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string Generate<T>(T payload) where T : TokenDataModel
        {
            var token = JwtEncoder.Encode(payload, payload.ExpireTime, (x, writer) => writer.Write(Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(x))));

            return token;
        }

        /// <summary>
        ///     Get Token Payload Data
        /// </summary>
        /// <param name="token"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>null if invalid or expired Token</returns>
        public static T Get<T>(string token) where T : TokenDataModel
        {
            try
            {
                var result = JwtDecoder.TryDecode(token, x => JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(x)), out var payload);

                return result != DecodeResult.Success ? default : payload;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
                return default;
            }
        }
    }
}