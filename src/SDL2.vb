#Region "Using Statements"
Imports System
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports System.Text
#End Region

Namespace SDL2
    Public Module SDL
#Region "SDL2# Variables"

        Private Const nativeLibName As String = "SDL2.dll"


#End Region

#Region "UTF8 Marshaling"

        ' Used for stack allocated string marshaling. 
        Friend Function Utf8Size(ByVal str As String) As Integer
            Debug.Assert(Not Equals(str, Nothing))
            Return str.Length * 4 + 1
        End Function

        Friend Function Utf8SizeNullable(ByVal str As String) As Integer
            Return If(Not Equals(str, Nothing), str.Length * 4 + 1, 0)

            ' Used for heap allocated string marshaling.
            ' 		 * Returned byte* must be free'd with FreeHGlobal.
            ' 		 

            ' This is public because SDL_DropEvent needs it! 

            ' We get to do strlen ourselves! 

            ' TODO: This #ifdef is only here because the equivalent
            ' 			 * .NET 2.0 constructor appears to be less efficient?
            ' 			 * Here's the pretty version, maybe steal this instead:
            ' 			 *
            ' 			string result = new string(
            ' 				(sbyte*) s, // Also, why sbyte???
            ' 				0,
            ' 				(int) (ptr - (byte*) s),
            ' 				System.Text.Encoding.UTF8
            ' 			);
            ' 			 * See the CoreCLR source for more info.
            ' 			 * -flibit
            ' 			 
#If NETSTANDARD2_0 Then
			/* Modern C# lets you just send the byte*, nice! */
			string result = System.Text.Encoding.UTF8.GetString(
				(byte*) s,
				(int) (ptr - (byte*) s)
			);
#Else
            ' Old C# requires an extra memcpy, bleh! 
#End If

            ' Some SDL functions will malloc, we have to free! 
        End Function

        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		internal static unsafe byte* Utf8Encode(string str, byte* buffer, int bufferSize)
        ''' 		{
        ''' 			System.Diagnostics.Debug.Assert(str != null);
        ''' 			fixed (char* strPtr = str)
        ''' 			{
        ''' 				System.Text.Encoding.UTF8.GetBytes(strPtr, str.Length + 1, buffer, bufferSize);
        ''' 			}
        ''' 			return buffer;
        ''' 		}
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		internal static unsafe byte* Utf8EncodeNullable(string str, byte* buffer, int bufferSize)
        ''' 		{
        ''' 			if (str == null)
        ''' 			{
        ''' 				buffer[0] = 0;
        ''' 				return buffer;
        ''' 			}
        ''' 			fixed (char* strPtr = str)
        ''' 			{
        ''' 				System.Text.Encoding.UTF8.GetBytes(strPtr, str.Length + 1, buffer, bufferSize);
        ''' 			}
        ''' 			return buffer;
        ''' 		}
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* Used for heap allocated string marshaling.
        ''' 		 * Returned byte* must be free'd with FreeHGlobal.
        ''' 		 */
        ''' 		internal static unsafe byte* Utf8Encode(string str)
        ''' 		{
        ''' 			System.Diagnostics.Debug.Assert(str != null);
        ''' 			int bufferSize = SDL2.SDL.Utf8Size(str);
        ''' 			byte* buffer = (byte*)System.Runtime.InteropServices.Marshal.AllocHGlobal(bufferSize);
        ''' 			fixed (char* strPtr = str)
        ''' 			{
        ''' 				System.Text.Encoding.UTF8.GetBytes(strPtr, str.Length + 1, buffer, bufferSize);
        ''' 			}
        ''' 			return buffer;
        ''' 		}
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		internal static unsafe byte* Utf8EncodeNullable(string str)
        ''' 		{
        ''' 			int bufferSize = SDL2.SDL.Utf8SizeNullable(str);
        ''' 			byte* buffer = (byte*)System.Runtime.InteropServices.Marshal.AllocHGlobal(bufferSize);
        ''' 			fixed (char* strPtr = str)
        ''' 			{
        ''' 				System.Text.Encoding.UTF8.GetBytes(
        ''' 					strPtr,
        ''' 					(str != null) ? (str.Length + 1) : 0,
        ''' 					buffer,
        ''' 					bufferSize
        ''' 				);
        ''' 			}
        ''' 			return buffer;
        ''' 		}
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* This is public because SDL_DropEvent needs it! */
        ''' 		public static unsafe string UTF8_ToManaged(System.IntPtr s, bool freePtr = false)
        ''' 		{
        ''' 			if (s == System.IntPtr.Zero)
        ''' 			{
        ''' 				return null;
        ''' 			}
        ''' 
        ''' 			/* We get to do strlen ourselves! */
        ''' 			byte* ptr = (byte*)s;
        ''' 			while (*ptr != 0)
        ''' 			{
        ''' 				ptr++;
        ''' 			}
        ''' 
        ''' 			/* TODO: This #ifdef is only here because the equivalent
        ''' 			 * .NET 2.0 constructor appears to be less efficient?
        ''' 			 * Here's the pretty version, maybe steal this instead:
        ''' 			 *
        ''' 			string result = new string(
        ''' 				(sbyte*) s, // Also, why sbyte???
        ''' 				0,
        ''' 				(int) (ptr - (byte*) s),
        ''' 				System.Text.Encoding.UTF8
        ''' 			);
        ''' 			 * See the CoreCLR source for more info.
        ''' 			 * -flibit
        ''' 			 */
        ''' #if NETSTANDARD2_0
        ''' 			/* Modern C# lets you just send the byte*, nice! */
        ''' 			string result = System.Text.Encoding.UTF8.GetString(
        ''' 				(byte*) s,
        ''' 				(int) (ptr - (byte*) s)
        ''' 			);
        ''' #else
        ''' 			/* Old C# requires an extra memcpy, bleh! */
        ''' 			int len = (int)(ptr - (byte*)s);
        ''' 			if (len == 0)
        ''' 			{
        ''' 				return string.Empty;
        ''' 			}
        ''' 			char* chars = stackalloc char[len];
        ''' 			int strLen = System.Text.Encoding.UTF8.GetChars((byte*)s, len, chars, len);
        ''' 			string result = new string(chars, 0, strLen);
        ''' #endif
        ''' 
        ''' 			/* Some SDL functions will malloc, we have to free! */
        ''' 			if (freePtr)
        ''' 			{
        ''' 				SDL2.SDL.SDL_free(s);
        ''' 			}
        ''' 			return result;
        ''' 		}
        ''' 
        ''' 

        Public Function UTF8_ToNative(ByVal strInput As String) As Byte()
            Try
                Return System.Text.Encoding.UTF8.GetBytes(strInput & vbNullChar)
            Catch ex As Exception
                Return {vbNullChar}
            End Try
        End Function

        'Check this function to see if the string return is right.
        Public Function UTF8_ToManaged(ByRef s As IntPtr, Optional ByVal freeptr As Boolean = False) As String
            If s = IntPtr.Zero Then
                Return vbNullChar
            End If
            'notes: interesting find, this is obviously not a direct conversion
            Dim bufstring As String = Marshal.PtrToStringAuto(s)

            If freeptr Then
                SDL_free(s)
            End If

            Return bufstring
        End Function

#End Region

#Region "SDL_stdinc.h"

        Public Function SDL_FOURCC(ByVal A As Byte, ByVal B As Byte, ByVal C As Byte, ByVal D As Byte) As UInteger
            Return A Or B << 8 Or C << 16 Or D << 24
        End Function

        Public Enum SDL_bool
            SDL_FALSE = 0
            SDL_TRUE = 1
        End Enum


        ' malloc/free are used by the marshaler! -flibit 

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Friend Function SDL_malloc(ByVal size As IntPtr) As IntPtr
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Friend Sub SDL_free(ByVal memblock As IntPtr)
        End Sub


        ' Buffer.BlockCopy is not available in every runtime yet. Also,
        ' 		 * using memcpy directly can be a compatibility issue in other
        ' 		 * strange ways. So, we expose this to get around all that.
        ' 		 * -flibit
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_memcpy(ByVal dst As IntPtr, ByVal src As IntPtr, ByVal len As IntPtr) As IntPtr
        End Function


#End Region

#Region "SDL_rwops.h"

        Public Const RW_SEEK_SET As Integer = 0
        Public Const RW_SEEK_CUR As Integer = 1
        Public Const RW_SEEK_END As Integer = 2
        Public Const SDL_RWOPS_UNKNOWN As UInteger = 0 ' Unknown stream type 
        Public Const SDL_RWOPS_WINFILE As UInteger = 1 ' Win32 file 
        Public Const SDL_RWOPS_STDFILE As UInteger = 2 ' Stdio file 
        Public Const SDL_RWOPS_JNIFILE As UInteger = 3 ' Android asset 
        Public Const SDL_RWOPS_MEMORY As UInteger = 4 ' Memory stream 
        Public Const SDL_RWOPS_MEMORY_RO As UInteger = 5 ' Read-Only memory stream 
        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function SDLRWopsSizeCallback(ByVal context As IntPtr) As Long
        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function SDLRWopsSeekCallback(ByVal context As IntPtr, ByVal offset As Long, ByVal whence As Integer) As Long
        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function SDLRWopsReadCallback(ByVal context As IntPtr, ByVal ptr As IntPtr, ByVal size As IntPtr, ByVal maxnum As IntPtr) As IntPtr
        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function SDLRWopsWriteCallback(ByVal context As IntPtr, ByVal ptr As IntPtr, ByVal size As IntPtr, ByVal num As IntPtr) As IntPtr
        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function SDLRWopsCloseCallback(ByVal context As IntPtr) As Integer

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_RWops
            Public size As IntPtr
            Public seek As IntPtr
            Public read As IntPtr
            Public write As IntPtr
            Public close As IntPtr
            Public type As UInteger

            ' NOTE: This isn't the full structure since
            ' 			 * the native SDL_RWops contains a hidden union full of
            ' 			 * internal information and platform-specific stuff depending
            ' 			 * on what conditions the native library was built with
            ' 			 

            ' IntPtr refers to an SDL_RWops* 
        End Structure

        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* IntPtr refers to an SDL_RWops* */
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_RWFromFile", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe System.IntPtr INTERNAL_SDL_RWFromFile(
        ''' 			byte* file,
        ''' 			byte* mode
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe System.IntPtr SDL_RWFromFile(
        ''' 			string file,
        ''' 			string mode
        ''' 		)
        ''' 		{
        ''' 			byte* utf8File = SDL2.SDL.Utf8Encode(file);
        ''' 			byte* utf8Mode = SDL2.SDL.Utf8Encode(mode);
        ''' 			System.IntPtr rwOps = SDL2.SDL.INTERNAL_SDL_RWFromFile(
        ''' 				utf8File,
        ''' 				utf8Mode
        ''' 			);
        ''' 			System.Runtime.InteropServices.Marshal.FreeHGlobal((System.IntPtr)utf8Mode);
        ''' 			System.Runtime.InteropServices.Marshal.FreeHGlobal((System.IntPtr)utf8File);
        ''' 			return rwOps;
        ''' 		}
        ''' 
        ''' 

        <DllImport(nativeLibName, EntryPoint:="SDL_RWFromFile", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_RWFromFile(ByVal file() As Byte, ByVal mode() As Byte) As IntPtr
        End Function
        Public Function SDL_RWFromFile(ByVal file As String, ByVal mode As String) As IntPtr
            Return INTERNAL_SDL_RWFromFile(UTF8_ToNative(file), UTF8_ToNative(mode))
        End Function

        ' IntPtr refers to an SDL_RWops* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_AllocRW() As IntPtr
        End Function


        ' area refers to an SDL_RWops* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_FreeRW(ByVal area As IntPtr)
        End Sub


        ' fp refers to a void* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RWFromFP(ByVal fp As IntPtr, ByVal autoclose As SDL_bool) As IntPtr
        End Function


        ' mem refers to a void*, IntPtr to an SDL_RWops* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RWFromMem(ByVal mem As IntPtr, ByVal size As Integer) As IntPtr
        End Function


        ' mem refers to a const void*, IntPtr to an SDL_RWops* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RWFromConstMem(ByVal mem As IntPtr, ByVal size As Integer) As IntPtr
        End Function


        ' context refers to an SDL_RWops*.
        ' 		 * Only available in SDL 2.0.10 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RWsize(ByVal context As IntPtr) As Long
        End Function


        ' context refers to an SDL_RWops*.
        ' 		 * Only available in SDL 2.0.10 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RWseek(ByVal context As IntPtr, ByVal offset As Long, ByVal whence As Integer) As Long
        End Function


        ' context refers to an SDL_RWops*.
        ' 		 * Only available in SDL 2.0.10 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RWtell(ByVal context As IntPtr) As Long
        End Function


        ' context refers to an SDL_RWops*, ptr refers to a void*.
        ' 		 * Only available in SDL 2.0.10 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RWread(ByVal context As IntPtr, ByVal ptr As IntPtr, ByVal size As IntPtr, ByVal maxnum As IntPtr) As Long
        End Function


        ' context refers to an SDL_RWops*, ptr refers to a const void*.
        ' 		 * Only available in SDL 2.0.10 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RWwrite(ByVal context As IntPtr, ByVal ptr As IntPtr, ByVal size As IntPtr, ByVal maxnum As IntPtr) As Long
        End Function


        ' Read endian functions 

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_ReadU8(ByVal src As IntPtr) As Byte
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_ReadLE16(ByVal src As IntPtr) As UShort
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_ReadBE16(ByVal src As IntPtr) As UShort
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_ReadLE32(ByVal src As IntPtr) As UInteger
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_ReadBE32(ByVal src As IntPtr) As UInteger
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_ReadLE64(ByVal src As IntPtr) As ULong
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_ReadBE64(ByVal src As IntPtr) As ULong
        End Function


        ' Write endian functions 

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_WriteU8(ByVal dst As IntPtr, ByVal value As Byte) As UInteger
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_WriteLE16(ByVal dst As IntPtr, ByVal value As UShort) As UInteger
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_WriteBE16(ByVal dst As IntPtr, ByVal value As UShort) As UInteger
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_WriteLE32(ByVal dst As IntPtr, ByVal value As UInteger) As UInteger
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_WriteBE32(ByVal dst As IntPtr, ByVal value As UInteger) As UInteger
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_WriteLE64(ByVal dst As IntPtr, ByVal value As ULong) As UInteger
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_WriteBE64(ByVal dst As IntPtr, ByVal value As ULong) As UInteger
        End Function


        ' context refers to an SDL_RWops*
        ' 		 * Only available in SDL 2.0.10 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RWclose(ByVal context As IntPtr) As Long
        End Function


        ' file refers to a const char*, datasize to a size_t*
        ' 		 * IntPtr refers to a void*
        ' 		 * Only available in SDL 2.0.10 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_LoadFile(ByVal file As IntPtr, ByVal datasize As IntPtr) As IntPtr
        End Function


#End Region

#Region "SDL_main.h"

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_SetMainReady()
        End Sub


        ' This is used as a function pointer to a C main() function 
        Public Delegate Function SDL_main_func(ByVal argc As Integer, ByVal argv As IntPtr) As Integer


        ' Use this function with UWP to call your C# Main() function! 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_WinRTRunApp(ByVal mainFunction As SDL_main_func, ByVal reserved As IntPtr) As Integer
        End Function


        ' Use this function with iOS to call your C# Main() function!
        ' 		 * Only available in SDL 2.0.10 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_UIKitRunApp(ByVal argc As Integer, ByVal argv As IntPtr, ByVal mainFunction As SDL_main_func) As Integer
        End Function


#End Region

#Region "SDL.h"

        Public Const SDL_INIT_TIMER As UInteger = &H1
        Public Const SDL_INIT_AUDIO As UInteger = &H10
        Public Const SDL_INIT_VIDEO As UInteger = &H20
        Public Const SDL_INIT_JOYSTICK As UInteger = &H200
        Public Const SDL_INIT_HAPTIC As UInteger = &H1000
        Public Const SDL_INIT_GAMECONTROLLER As UInteger = &H2000
        Public Const SDL_INIT_EVENTS As UInteger = &H4000
        Public Const SDL_INIT_SENSOR As UInteger = &H8000
        Public Const SDL_INIT_NOPARACHUTE As UInteger = &H100000
        Public Const SDL_INIT_EVERYTHING As UInteger = SDL_INIT_TIMER Or SDL_INIT_AUDIO Or SDL_INIT_VIDEO Or SDL_INIT_EVENTS Or SDL_INIT_JOYSTICK Or SDL_INIT_HAPTIC Or SDL_INIT_GAMECONTROLLER Or SDL_INIT_SENSOR

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_Init(ByVal flags As UInteger) As Integer
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_InitSubSystem(ByVal flags As UInteger) As Integer
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_Quit()
        End Sub

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_QuitSubSystem(ByVal flags As UInteger)
        End Sub

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_WasInit(ByVal flags As UInteger) As UInteger
        End Function


#End Region

#Region "SDL_platform.h"

        <DllImport(nativeLibName, EntryPoint:="SDL_GetPlatform", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GetPlatform() As IntPtr
        End Function

        Public Function SDL_GetPlatform() As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GetPlatform())
        End Function


#End Region

#Region "SDL_hints.h"

        Public Const SDL_HINT_FRAMEBUFFER_ACCELERATION As String = "SDL_FRAMEBUFFER_ACCELERATION"
        Public Const SDL_HINT_RENDER_DRIVER As String = "SDL_RENDER_DRIVER"
        Public Const SDL_HINT_RENDER_OPENGL_SHADERS As String = "SDL_RENDER_OPENGL_SHADERS"
        Public Const SDL_HINT_RENDER_DIRECT3D_THREADSAFE As String = "SDL_RENDER_DIRECT3D_THREADSAFE"
        Public Const SDL_HINT_RENDER_VSYNC As String = "SDL_RENDER_VSYNC"
        Public Const SDL_HINT_VIDEO_X11_XVIDMODE As String = "SDL_VIDEO_X11_XVIDMODE"
        Public Const SDL_HINT_VIDEO_X11_XINERAMA As String = "SDL_VIDEO_X11_XINERAMA"
        Public Const SDL_HINT_VIDEO_X11_XRANDR As String = "SDL_VIDEO_X11_XRANDR"
        Public Const SDL_HINT_GRAB_KEYBOARD As String = "SDL_GRAB_KEYBOARD"
        Public Const SDL_HINT_VIDEO_MINIMIZE_ON_FOCUS_LOSS As String = "SDL_VIDEO_MINIMIZE_ON_FOCUS_LOSS"
        Public Const SDL_HINT_IDLE_TIMER_DISABLED As String = "SDL_IOS_IDLE_TIMER_DISABLED"
        Public Const SDL_HINT_ORIENTATIONS As String = "SDL_IOS_ORIENTATIONS"
        Public Const SDL_HINT_XINPUT_ENABLED As String = "SDL_XINPUT_ENABLED"
        Public Const SDL_HINT_GAMECONTROLLERCONFIG As String = "SDL_GAMECONTROLLERCONFIG"
        Public Const SDL_HINT_JOYSTICK_ALLOW_BACKGROUND_EVENTS As String = "SDL_JOYSTICK_ALLOW_BACKGROUND_EVENTS"
        Public Const SDL_HINT_ALLOW_TOPMOST As String = "SDL_ALLOW_TOPMOST"
        Public Const SDL_HINT_TIMER_RESOLUTION As String = "SDL_TIMER_RESOLUTION"
        Public Const SDL_HINT_RENDER_SCALE_QUALITY As String = "SDL_RENDER_SCALE_QUALITY"

        ' Only available in SDL 2.0.1 or higher. 
        Public Const SDL_HINT_VIDEO_HIGHDPI_DISABLED As String = "SDL_VIDEO_HIGHDPI_DISABLED"

        ' Only available in SDL 2.0.2 or higher. 
        Public Const SDL_HINT_CTRL_CLICK_EMULATE_RIGHT_CLICK As String = "SDL_CTRL_CLICK_EMULATE_RIGHT_CLICK"
        Public Const SDL_HINT_VIDEO_WIN_D3DCOMPILER As String = "SDL_VIDEO_WIN_D3DCOMPILER"
        Public Const SDL_HINT_MOUSE_RELATIVE_MODE_WARP As String = "SDL_MOUSE_RELATIVE_MODE_WARP"
        Public Const SDL_HINT_VIDEO_WINDOW_SHARE_PIXEL_FORMAT As String = "SDL_VIDEO_WINDOW_SHARE_PIXEL_FORMAT"
        Public Const SDL_HINT_VIDEO_ALLOW_SCREENSAVER As String = "SDL_VIDEO_ALLOW_SCREENSAVER"
        Public Const SDL_HINT_ACCELEROMETER_AS_JOYSTICK As String = "SDL_ACCELEROMETER_AS_JOYSTICK"
        Public Const SDL_HINT_VIDEO_MAC_FULLSCREEN_SPACES As String = "SDL_VIDEO_MAC_FULLSCREEN_SPACES"

        ' Only available in SDL 2.0.3 or higher. 
        Public Const SDL_HINT_WINRT_PRIVACY_POLICY_URL As String = "SDL_WINRT_PRIVACY_POLICY_URL"
        Public Const SDL_HINT_WINRT_PRIVACY_POLICY_LABEL As String = "SDL_WINRT_PRIVACY_POLICY_LABEL"
        Public Const SDL_HINT_WINRT_HANDLE_BACK_BUTTON As String = "SDL_WINRT_HANDLE_BACK_BUTTON"

        ' Only available in SDL 2.0.4 or higher. 
        Public Const SDL_HINT_NO_SIGNAL_HANDLERS As String = "SDL_NO_SIGNAL_HANDLERS"
        Public Const SDL_HINT_IME_INTERNAL_EDITING As String = "SDL_IME_INTERNAL_EDITING"
        Public Const SDL_HINT_ANDROID_SEPARATE_MOUSE_AND_TOUCH As String = "SDL_ANDROID_SEPARATE_MOUSE_AND_TOUCH"
        Public Const SDL_HINT_EMSCRIPTEN_KEYBOARD_ELEMENT As String = "SDL_EMSCRIPTEN_KEYBOARD_ELEMENT"
        Public Const SDL_HINT_THREAD_STACK_SIZE As String = "SDL_THREAD_STACK_SIZE"
        Public Const SDL_HINT_WINDOW_FRAME_USABLE_WHILE_CURSOR_HIDDEN As String = "SDL_WINDOW_FRAME_USABLE_WHILE_CURSOR_HIDDEN"
        Public Const SDL_HINT_WINDOWS_ENABLE_MESSAGELOOP As String = "SDL_WINDOWS_ENABLE_MESSAGELOOP"
        Public Const SDL_HINT_WINDOWS_NO_CLOSE_ON_ALT_F4 As String = "SDL_WINDOWS_NO_CLOSE_ON_ALT_F4"
        Public Const SDL_HINT_XINPUT_USE_OLD_JOYSTICK_MAPPING As String = "SDL_XINPUT_USE_OLD_JOYSTICK_MAPPING"
        Public Const SDL_HINT_MAC_BACKGROUND_APP As String = "SDL_MAC_BACKGROUND_APP"
        Public Const SDL_HINT_VIDEO_X11_NET_WM_PING As String = "SDL_VIDEO_X11_NET_WM_PING"
        Public Const SDL_HINT_ANDROID_APK_EXPANSION_MAIN_FILE_VERSION As String = "SDL_ANDROID_APK_EXPANSION_MAIN_FILE_VERSION"
        Public Const SDL_HINT_ANDROID_APK_EXPANSION_PATCH_FILE_VERSION As String = "SDL_ANDROID_APK_EXPANSION_PATCH_FILE_VERSION"

        ' Only available in 2.0.5 or higher. 
        Public Const SDL_HINT_MOUSE_FOCUS_CLICKTHROUGH As String = "SDL_MOUSE_FOCUS_CLICKTHROUGH"
        Public Const SDL_HINT_BMP_SAVE_LEGACY_FORMAT As String = "SDL_BMP_SAVE_LEGACY_FORMAT"
        Public Const SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING As String = "SDL_WINDOWS_DISABLE_THREAD_NAMING"
        Public Const SDL_HINT_APPLE_TV_REMOTE_ALLOW_ROTATION As String = "SDL_APPLE_TV_REMOTE_ALLOW_ROTATION"

        ' Only available in 2.0.6 or higher. 
        Public Const SDL_HINT_AUDIO_RESAMPLING_MODE As String = "SDL_AUDIO_RESAMPLING_MODE"
        Public Const SDL_HINT_RENDER_LOGICAL_SIZE_MODE As String = "SDL_RENDER_LOGICAL_SIZE_MODE"
        Public Const SDL_HINT_MOUSE_NORMAL_SPEED_SCALE As String = "SDL_MOUSE_NORMAL_SPEED_SCALE"
        Public Const SDL_HINT_MOUSE_RELATIVE_SPEED_SCALE As String = "SDL_MOUSE_RELATIVE_SPEED_SCALE"
        Public Const SDL_HINT_TOUCH_MOUSE_EVENTS As String = "SDL_TOUCH_MOUSE_EVENTS"
        Public Const SDL_HINT_WINDOWS_INTRESOURCE_ICON As String = "SDL_WINDOWS_INTRESOURCE_ICON"
        Public Const SDL_HINT_WINDOWS_INTRESOURCE_ICON_SMALL As String = "SDL_WINDOWS_INTRESOURCE_ICON_SMALL"

        ' Only available in 2.0.8 or higher. 
        Public Const SDL_HINT_IOS_HIDE_HOME_INDICATOR As String = "SDL_IOS_HIDE_HOME_INDICATOR"
        Public Const SDL_HINT_TV_REMOTE_AS_JOYSTICK As String = "SDL_TV_REMOTE_AS_JOYSTICK"

        ' Only available in 2.0.9 or higher. 
        Public Const SDL_HINT_MOUSE_DOUBLE_CLICK_TIME As String = "SDL_MOUSE_DOUBLE_CLICK_TIME"
        Public Const SDL_HINT_MOUSE_DOUBLE_CLICK_RADIUS As String = "SDL_MOUSE_DOUBLE_CLICK_RADIUS"
        Public Const SDL_HINT_JOYSTICK_HIDAPI As String = "SDL_JOYSTICK_HIDAPI"
        Public Const SDL_HINT_JOYSTICK_HIDAPI_PS4 As String = "SDL_JOYSTICK_HIDAPI_PS4"
        Public Const SDL_HINT_JOYSTICK_HIDAPI_PS4_RUMBLE As String = "SDL_JOYSTICK_HIDAPI_PS4_RUMBLE"
        Public Const SDL_HINT_JOYSTICK_HIDAPI_STEAM As String = "SDL_JOYSTICK_HIDAPI_STEAM"
        Public Const SDL_HINT_JOYSTICK_HIDAPI_SWITCH As String = "SDL_JOYSTICK_HIDAPI_SWITCH"
        Public Const SDL_HINT_JOYSTICK_HIDAPI_XBOX As String = "SDL_JOYSTICK_HIDAPI_XBOX"
        Public Const SDL_HINT_ENABLE_STEAM_CONTROLLERS As String = "SDL_ENABLE_STEAM_CONTROLLERS"
        Public Const SDL_HINT_ANDROID_TRAP_BACK_BUTTON As String = "SDL_ANDROID_TRAP_BACK_BUTTON"

        ' Only available in 2.0.10 or higher. 
        Public Const SDL_HINT_MOUSE_TOUCH_EVENTS As String = "SDL_MOUSE_TOUCH_EVENTS"
        Public Const SDL_HINT_GAMECONTROLLERCONFIG_FILE As String = "SDL_GAMECONTROLLERCONFIG_FILE"
        Public Const SDL_HINT_ANDROID_BLOCK_ON_PAUSE As String = "SDL_ANDROID_BLOCK_ON_PAUSE"
        Public Const SDL_HINT_RENDER_BATCHING As String = "SDL_RENDER_BATCHING"
        Public Const SDL_HINT_EVENT_LOGGING As String = "SDL_EVENT_LOGGING"
        Public Const SDL_HINT_WAVE_RIFF_CHUNK_SIZE As String = "SDL_WAVE_RIFF_CHUNK_SIZE"
        Public Const SDL_HINT_WAVE_TRUNCATION As String = "SDL_WAVE_TRUNCATION"
        Public Const SDL_HINT_WAVE_FACT_CHUNK As String = "SDL_WAVE_FACT_CHUNK"

        ' Only available in 2.0.11 or higher. 
        Public Const SDL_HINT_VIDO_X11_WINDOW_VISUALID As String = "SDL_VIDEO_X11_WINDOW_VISUALID"
        Public Const SDL_HINT_GAMECONTROLLER_USE_BUTTON_LABELS As String = "SDL_GAMECONTROLLER_USE_BUTTON_LABELS"
        Public Const SDL_HINT_VIDEO_EXTERNAL_CONTEXT As String = "SDL_VIDEO_EXTERNAL_CONTEXT"
        Public Const SDL_HINT_JOYSTICK_HIDAPI_GAMECUBE As String = "SDL_JOYSTICK_HIDAPI_GAMECUBE"
        Public Const SDL_HINT_DISPLAY_USABLE_BOUNDS As String = "SDL_DISPLAY_USABLE_BOUNDS"
        Public Const SDL_HINT_VIDEO_X11_FORCE_EGL As String = "SDL_VIDEO_X11_FORCE_EGL"
        Public Const SDL_HINT_GAMECONTROLLERTYPE As String = "SDL_GAMECONTROLLERTYPE"

        Public Enum SDL_HintPriority
            SDL_HINT_DEFAULT
            SDL_HINT_NORMAL
            SDL_HINT_OVERRIDE
        End Enum

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_ClearHints()
        End Sub

        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_GetHint", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe System.IntPtr INTERNAL_SDL_GetHint(byte* name);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe string SDL_GetHint(string name)
        ''' 		{
        ''' 			int utf8NameBufSize = SDL2.SDL.Utf8Size(name);
        ''' 			byte* utf8Name = stackalloc byte[utf8NameBufSize];
        ''' 			return SDL2.SDL.UTF8_ToManaged(
        ''' 				SDL2.SDL.INTERNAL_SDL_GetHint(
        ''' 					SDL2.SDL.Utf8Encode(name, utf8Name, utf8NameBufSize)
        ''' 				)
        ''' 			);
        ''' 		}
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_SetHint", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe SDL2.SDL.SDL_bool INTERNAL_SDL_SetHint(
        ''' 			byte* name,
        ''' 			byte* value
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe SDL2.SDL.SDL_bool SDL_SetHint(string name, string value)
        ''' 		{
        ''' 			int utf8NameBufSize = SDL2.SDL.Utf8Size(name);
        ''' 			byte* utf8Name = stackalloc byte[utf8NameBufSize];
        ''' 
        ''' 			int utf8ValueBufSize = SDL2.SDL.Utf8Size(value);
        ''' 			byte* utf8Value = stackalloc byte[utf8ValueBufSize];
        ''' 
        ''' 			return SDL2.SDL.INTERNAL_SDL_SetHint(
        ''' 				SDL2.SDL.Utf8Encode(name, utf8Name, utf8NameBufSize),
        ''' 				SDL2.SDL.Utf8Encode(value, utf8Value, utf8ValueBufSize)
        ''' 			);
        ''' 		}
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_SetHintWithPriority", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe SDL2.SDL.SDL_bool INTERNAL_SDL_SetHintWithPriority(
        ''' 			byte* name,
        ''' 			byte* value,
        ''' 			SDL2.SDL.SDL_HintPriority priority
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe SDL2.SDL.SDL_bool SDL_SetHintWithPriority(
        ''' 			string name,
        ''' 			string value,
        ''' 			SDL2.SDL.SDL_HintPriority priority
        ''' 		)
        ''' 		{
        ''' 			int utf8NameBufSize = SDL2.SDL.Utf8Size(name);
        ''' 			byte* utf8Name = stackalloc byte[utf8NameBufSize];
        ''' 
        ''' 			int utf8ValueBufSize = SDL2.SDL.Utf8Size(value);
        ''' 			byte* utf8Value = stackalloc byte[utf8ValueBufSize];
        ''' 
        ''' 			return SDL2.SDL.INTERNAL_SDL_SetHintWithPriority(
        ''' 				SDL2.SDL.Utf8Encode(name, utf8Name, utf8NameBufSize),
        ''' 				SDL2.SDL.Utf8Encode(value, utf8Value, utf8ValueBufSize),
        ''' 				priority
        ''' 			);
        ''' 		}
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* Only available in 2.0.5 or higher. */
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_GetHintBoolean", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe SDL2.SDL.SDL_bool INTERNAL_SDL_GetHintBoolean(
        ''' 			byte* name,
        ''' 			SDL2.SDL.SDL_bool default_value
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe SDL2.SDL.SDL_bool SDL_GetHintBoolean(
        ''' 			string name,
        ''' 			SDL2.SDL.SDL_bool default_value
        ''' 		)
        ''' 		{
        ''' 			int utf8NameBufSize = SDL2.SDL.Utf8Size(name);
        ''' 			byte* utf8Name = stackalloc byte[utf8NameBufSize];
        ''' 			return SDL2.SDL.INTERNAL_SDL_GetHintBoolean(
        ''' 				SDL2.SDL.Utf8Encode(name, utf8Name, utf8NameBufSize),
        ''' 				default_value
        ''' 			);
        ''' 		}
        ''' 
        ''' 

#End Region

#Region "SDL_error.h"

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_ClearError()
        End Sub

        <DllImport(nativeLibName, EntryPoint:="SDL_GetError", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GetError() As IntPtr
        End Function

        Public Function SDL_GetError() As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GetError())

            ' Use string.Format for arglists 
        End Function

        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* Use string.Format for arglists */
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_SetError", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe void INTERNAL_SDL_SetError(byte* fmtAndArglist);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe void SDL_SetError(string fmtAndArglist)
        ''' 		{
        ''' 			int utf8FmtAndArglistBufSize = SDL2.SDL.Utf8Size(fmtAndArglist);
        ''' 			byte* utf8FmtAndArglist = stackalloc byte[utf8FmtAndArglistBufSize];
        ''' 			SDL2.SDL.INTERNAL_SDL_SetError(
        ''' 				SDL2.SDL.Utf8Encode(fmtAndArglist, utf8FmtAndArglist, utf8FmtAndArglistBufSize)
        ''' 			);
        ''' 		}
        ''' 
        ''' 

#End Region

#Region "SDL_log.h"

        Public Enum SDL_LogCategory
            SDL_LOG_CATEGORY_APPLICATION
            SDL_LOG_CATEGORY_ERROR
            SDL_LOG_CATEGORY_ASSERT
            SDL_LOG_CATEGORY_SYSTEM
            SDL_LOG_CATEGORY_AUDIO
            SDL_LOG_CATEGORY_VIDEO
            SDL_LOG_CATEGORY_RENDER
            SDL_LOG_CATEGORY_INPUT
            SDL_LOG_CATEGORY_TEST

            ' Reserved for future SDL library use 
            SDL_LOG_CATEGORY_RESERVED1
            SDL_LOG_CATEGORY_RESERVED2
            SDL_LOG_CATEGORY_RESERVED3
            SDL_LOG_CATEGORY_RESERVED4
            SDL_LOG_CATEGORY_RESERVED5
            SDL_LOG_CATEGORY_RESERVED6
            SDL_LOG_CATEGORY_RESERVED7
            SDL_LOG_CATEGORY_RESERVED8
            SDL_LOG_CATEGORY_RESERVED9
            SDL_LOG_CATEGORY_RESERVED10

            ' Beyond this point is reserved for application use, e.g.
            ' 			enum {
            ' 				MYAPP_CATEGORY_AWESOME1 = SDL_LOG_CATEGORY_CUSTOM,
            ' 				MYAPP_CATEGORY_AWESOME2,
            ' 				MYAPP_CATEGORY_AWESOME3,
            ' 				...
            ' 			};
            ' 			
            SDL_LOG_CATEGORY_CUSTOM
        End Enum

        Public Enum SDL_LogPriority
            SDL_LOG_PRIORITY_VERBOSE = 1
            SDL_LOG_PRIORITY_DEBUG
            SDL_LOG_PRIORITY_INFO
            SDL_LOG_PRIORITY_WARN
            SDL_LOG_PRIORITY_ERROR
            SDL_LOG_PRIORITY_CRITICAL
            SDL_NUM_LOG_PRIORITIES
        End Enum


        ' userdata refers to a void*, message to a const char* 
        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub SDL_LogOutputFunction(ByVal userdata As IntPtr, ByVal category As Integer, ByVal priority As SDL_LogPriority, ByVal message As IntPtr)
        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* Use string.Format for arglists */
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_Log", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe void INTERNAL_SDL_Log(byte* fmtAndArglist);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe void SDL_Log(string fmtAndArglist)
        ''' 		{
        ''' 			int utf8FmtAndArglistBufSize = SDL2.SDL.Utf8Size(fmtAndArglist);
        ''' 			byte* utf8FmtAndArglist = stackalloc byte[utf8FmtAndArglistBufSize];
        ''' 			SDL2.SDL.INTERNAL_SDL_Log(
        ''' 				SDL2.SDL.Utf8Encode(fmtAndArglist, utf8FmtAndArglist, utf8FmtAndArglistBufSize)
        ''' 			);
        ''' 		}
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* Use string.Format for arglists */
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_LogVerbose", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe void INTERNAL_SDL_LogVerbose(
        ''' 			int category,
        ''' 			byte* fmtAndArglist
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe void SDL_LogVerbose(
        ''' 			int category,
        ''' 			string fmtAndArglist
        ''' 		)
        ''' 		{
        ''' 			int utf8FmtAndArglistBufSize = SDL2.SDL.Utf8Size(fmtAndArglist);
        ''' 			byte* utf8FmtAndArglist = stackalloc byte[utf8FmtAndArglistBufSize];
        ''' 			SDL2.SDL.INTERNAL_SDL_LogVerbose(
        ''' 				category,
        ''' 				SDL2.SDL.Utf8Encode(fmtAndArglist, utf8FmtAndArglist, utf8FmtAndArglistBufSize)
        ''' 			);
        ''' 		}
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* Use string.Format for arglists */
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_LogDebug", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe void INTERNAL_SDL_LogDebug(
        ''' 			int category,
        ''' 			byte* fmtAndArglist
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe void SDL_LogDebug(
        ''' 			int category,
        ''' 			string fmtAndArglist
        ''' 		)
        ''' 		{
        ''' 			int utf8FmtAndArglistBufSize = SDL2.SDL.Utf8Size(fmtAndArglist);
        ''' 			byte* utf8FmtAndArglist = stackalloc byte[utf8FmtAndArglistBufSize];
        ''' 			SDL2.SDL.INTERNAL_SDL_LogDebug(
        ''' 				category,
        ''' 				SDL2.SDL.Utf8Encode(fmtAndArglist, utf8FmtAndArglist, utf8FmtAndArglistBufSize)
        ''' 			);
        ''' 		}
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* Use string.Format for arglists */
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_LogInfo", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe void INTERNAL_SDL_LogInfo(
        ''' 			int category,
        ''' 			byte* fmtAndArglist
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe void SDL_LogInfo(
        ''' 			int category,
        ''' 			string fmtAndArglist
        ''' 		)
        ''' 		{
        ''' 			int utf8FmtAndArglistBufSize = SDL2.SDL.Utf8Size(fmtAndArglist);
        ''' 			byte* utf8FmtAndArglist = stackalloc byte[utf8FmtAndArglistBufSize];
        ''' 			SDL2.SDL.INTERNAL_SDL_LogInfo(
        ''' 				category,
        ''' 				SDL2.SDL.Utf8Encode(fmtAndArglist, utf8FmtAndArglist, utf8FmtAndArglistBufSize)
        ''' 			);
        ''' 		}
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* Use string.Format for arglists */
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_LogWarn", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe void INTERNAL_SDL_LogWarn(
        ''' 			int category,
        ''' 			byte* fmtAndArglist
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe void SDL_LogWarn(
        ''' 			int category,
        ''' 			string fmtAndArglist
        ''' 		)
        ''' 		{
        ''' 			int utf8FmtAndArglistBufSize = SDL2.SDL.Utf8Size(fmtAndArglist);
        ''' 			byte* utf8FmtAndArglist = stackalloc byte[utf8FmtAndArglistBufSize];
        ''' 			SDL2.SDL.INTERNAL_SDL_LogWarn(
        ''' 				category,
        ''' 				SDL2.SDL.Utf8Encode(fmtAndArglist, utf8FmtAndArglist, utf8FmtAndArglistBufSize)
        ''' 			);
        ''' 		}
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* Use string.Format for arglists */
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_LogError", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe void INTERNAL_SDL_LogError(
        ''' 			int category,
        ''' 			byte* fmtAndArglist
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe void SDL_LogError(
        ''' 			int category,
        ''' 			string fmtAndArglist
        ''' 		)
        ''' 		{
        ''' 			int utf8FmtAndArglistBufSize = SDL2.SDL.Utf8Size(fmtAndArglist);
        ''' 			byte* utf8FmtAndArglist = stackalloc byte[utf8FmtAndArglistBufSize];
        ''' 			SDL2.SDL.INTERNAL_SDL_LogError(
        ''' 				category,
        ''' 				SDL2.SDL.Utf8Encode(fmtAndArglist, utf8FmtAndArglist, utf8FmtAndArglistBufSize)
        ''' 			);
        ''' 		}
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* Use string.Format for arglists */
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_LogCritical", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe void INTERNAL_SDL_LogCritical(
        ''' 			int category,
        ''' 			byte* fmtAndArglist
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe void SDL_LogCritical(
        ''' 			int category,
        ''' 			string fmtAndArglist
        ''' 		)
        ''' 		{
        ''' 			int utf8FmtAndArglistBufSize = SDL2.SDL.Utf8Size(fmtAndArglist);
        ''' 			byte* utf8FmtAndArglist = stackalloc byte[utf8FmtAndArglistBufSize];
        ''' 			SDL2.SDL.INTERNAL_SDL_LogCritical(
        ''' 				category,
        ''' 				SDL2.SDL.Utf8Encode(fmtAndArglist, utf8FmtAndArglist, utf8FmtAndArglistBufSize)
        ''' 			);
        ''' 		}
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* Use string.Format for arglists */
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_LogMessage", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe void INTERNAL_SDL_LogMessage(
        ''' 			int category,
        ''' 			SDL2.SDL.SDL_LogPriority priority,
        ''' 			byte* fmtAndArglist
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe void SDL_LogMessage(
        ''' 			int category,
        ''' 			SDL2.SDL.SDL_LogPriority priority,
        ''' 			string fmtAndArglist
        ''' 		)
        ''' 		{
        ''' 			int utf8FmtAndArglistBufSize = SDL2.SDL.Utf8Size(fmtAndArglist);
        ''' 			byte* utf8FmtAndArglist = stackalloc byte[utf8FmtAndArglistBufSize];
        ''' 			SDL2.SDL.INTERNAL_SDL_LogMessage(
        ''' 				category,
        ''' 				priority,
        ''' 				SDL2.SDL.Utf8Encode(fmtAndArglist, utf8FmtAndArglist, utf8FmtAndArglistBufSize)
        ''' 			);
        ''' 		}
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* Use string.Format for arglists */
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_LogMessageV", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe void INTERNAL_SDL_LogMessageV(
        ''' 			int category,
        ''' 			SDL2.SDL.SDL_LogPriority priority,
        ''' 			byte* fmtAndArglist
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe void SDL_LogMessageV(
        ''' 			int category,
        ''' 			SDL2.SDL.SDL_LogPriority priority,
        ''' 			string fmtAndArglist
        ''' 		)
        ''' 		{
        ''' 			int utf8FmtAndArglistBufSize = SDL2.SDL.Utf8Size(fmtAndArglist);
        ''' 			byte* utf8FmtAndArglist = stackalloc byte[utf8FmtAndArglistBufSize];
        ''' 			SDL2.SDL.INTERNAL_SDL_LogMessageV(
        ''' 				category,
        ''' 				priority,
        ''' 				SDL2.SDL.Utf8Encode(fmtAndArglist, utf8FmtAndArglist, utf8FmtAndArglistBufSize)
        ''' 			);
        ''' 		}
        ''' 
        ''' 

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_LogGetPriority(ByVal category As Integer) As SDL_LogPriority
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_LogSetPriority(ByVal category As Integer, ByVal priority As SDL_LogPriority)
        End Sub

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_LogSetAllPriority(ByVal priority As SDL_LogPriority)
        End Sub

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_LogResetPriorities()
        End Sub


        ' userdata refers to a void* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Private Sub SDL_LogGetOutputFunction(<Out> ByRef callback As IntPtr, <Out> ByRef userdata As IntPtr)
        End Sub

        Public Sub SDL_LogGetOutputFunction(<Out> ByRef callback As SDL_LogOutputFunction, <Out> ByRef userdata As IntPtr)
            Dim result As IntPtr = IntPtr.Zero
            SDL_LogGetOutputFunction(result, userdata)

            If result <> IntPtr.Zero Then
                callback = CType(Marshal.GetDelegateForFunctionPointer(result, GetType(SDL_LogOutputFunction)), SDL_LogOutputFunction)
            Else
                callback = Nothing
            End If
        End Sub


        ' userdata refers to a void* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_LogSetOutputFunction(ByVal callback As SDL_LogOutputFunction, ByVal userdata As IntPtr)
        End Sub


#End Region

#Region "SDL_messagebox.h"

        <Flags>
        Public Enum SDL_MessageBoxFlags As UInteger
            SDL_MESSAGEBOX_ERROR = &H10
            SDL_MESSAGEBOX_WARNING = &H20
            SDL_MESSAGEBOX_INFORMATION = &H40
        End Enum

        <Flags>
        Public Enum SDL_MessageBoxButtonFlags As UInteger
            SDL_MESSAGEBOX_BUTTON_RETURNKEY_DEFAULT = &H1
            SDL_MESSAGEBOX_BUTTON_ESCAPEKEY_DEFAULT = &H2
        End Enum

        <StructLayout(LayoutKind.Sequential)>
        Private Structure INTERNAL_SDL_MessageBoxButtonData
            Public flags As SDL_MessageBoxButtonFlags
            Public buttonid As Integer
            Public text As IntPtr ' The UTF-8 button text 
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_MessageBoxButtonData
            Public flags As SDL_MessageBoxButtonFlags
            Public buttonid As Integer
            Public text As String ' The UTF-8 button text 
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_MessageBoxColor
            Public r, g, b As Byte
        End Structure

        Public Enum SDL_MessageBoxColorType
            SDL_MESSAGEBOX_COLOR_BACKGROUND
            SDL_MESSAGEBOX_COLOR_TEXT
            SDL_MESSAGEBOX_COLOR_BUTTON_BORDER
            SDL_MESSAGEBOX_COLOR_BUTTON_BACKGROUND
            SDL_MESSAGEBOX_COLOR_BUTTON_SELECTED
            SDL_MESSAGEBOX_COLOR_MAX
        End Enum

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_MessageBoxColorScheme
            <MarshalAs(UnmanagedType.ByValArray, ArraySubType:=UnmanagedType.Struct, SizeConst:=SDL_MessageBoxColorType.SDL_MESSAGEBOX_COLOR_MAX)>
            Public colors As SDL_MessageBoxColor()
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Private Structure INTERNAL_SDL_MessageBoxData
            Public flags As SDL_MessageBoxFlags
            Public window As IntPtr               ' Parent window, can be NULL 
            Public title As IntPtr                ' UTF-8 title 
            Public message As IntPtr              ' UTF-8 message text 
            Public numbuttons As Integer
            Public buttons As IntPtr
            Public colorScheme As IntPtr          ' Can be NULL to use system settings 
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_MessageBoxData
            Public flags As SDL_MessageBoxFlags
            Public window As IntPtr               ' Parent window, can be NULL 
            Public title As String                ' UTF-8 title 
            Public message As String              ' UTF-8 message text 
            Public numbuttons As Integer
            Public buttons As SDL_MessageBoxButtonData()
            Public colorScheme As SDL_MessageBoxColorScheme?  ' Can be NULL to use system settings 
        End Structure

        <DllImport(nativeLibName, EntryPoint:="SDL_ShowMessageBox", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_ShowMessageBox(
        <[In]()> ByRef messageboxdata As INTERNAL_SDL_MessageBoxData, <Out> ByRef buttonid As Integer) As Integer
        End Function


        ' Ripped from Jameson's LpUtf8StrMarshaler 
        Private Function INTERNAL_AllocUTF8(ByVal str As String) As IntPtr
            If String.IsNullOrEmpty(str) Then
                Return IntPtr.Zero
            End If

            Dim bytes As Byte() = Encoding.UTF8.GetBytes(str & ChrW(0))
            Dim mem As IntPtr = SDL_malloc(CType(bytes.Length, IntPtr))
            Marshal.Copy(bytes, 0, mem, bytes.Length)
            Return mem

            ' window refers to an SDL_Window* 
        End Function

        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		public static unsafe int SDL_ShowMessageBox([System.Runtime.InteropServices.@InAttribute()] ref SDL2.SDL.SDL_MessageBoxData messageboxdata, out int buttonid)
        ''' 		{
        ''' 			var data = new SDL2.SDL.INTERNAL_SDL_MessageBoxData()
        ''' 			{
        ''' 				flags = messageboxdata.flags,
        ''' 				window = messageboxdata.window,
        ''' 				title = SDL2.SDL.INTERNAL_AllocUTF8(messageboxdata.title),
        ''' 				message = SDL2.SDL.INTERNAL_AllocUTF8(messageboxdata.message),
        ''' 				numbuttons = messageboxdata.numbuttons,
        ''' 			};
        ''' 
        ''' 			var buttons = new SDL2.SDL.INTERNAL_SDL_MessageBoxButtonData[messageboxdata.numbuttons];
        ''' 			for (int i = 0; i < messageboxdata.numbuttons; i++)
        ''' 			{
        ''' 				buttons[i] = new SDL2.SDL.INTERNAL_SDL_MessageBoxButtonData()
        ''' 				{
        ''' 					flags = messageboxdata.buttons[(System.Int32)(i)].flags,
        ''' 					buttonid = messageboxdata.buttons[(System.Int32)(i)].buttonid,
        ''' 					text = SDL2.SDL.INTERNAL_AllocUTF8(messageboxdata.buttons[(System.Int32)(i)].text),
        ''' 				};
        ''' 			}
        ''' 
        ''' 			if (messageboxdata.colorScheme != null)
        ''' 			{
        ''' 				data.colorScheme = System.Runtime.InteropServices.Marshal.AllocHGlobal(System.Runtime.InteropServices.Marshal.SizeOf(typeof(SDL2.SDL.SDL_MessageBoxColorScheme)));
        ''' 				System.Runtime.InteropServices.Marshal.StructureToPtr(messageboxdata.colorScheme.Value, data.colorScheme, false);
        ''' 			}
        ''' 
        ''' 			int result;
        ''' 			fixed (SDL2.SDL.INTERNAL_SDL_MessageBoxButtonData* buttonsPtr = &buttons[0])
        ''' 			{
        ''' 				data.buttons = (System.IntPtr)buttonsPtr;
        ''' 				result = SDL2.SDL.INTERNAL_SDL_ShowMessageBox(ref data, out buttonid);
        ''' 			}
        ''' 
        ''' 			System.Runtime.InteropServices.Marshal.FreeHGlobal(data.colorScheme);
        ''' 			for (int i = 0; i < messageboxdata.numbuttons; i++)
        ''' 			{
        ''' 				SDL2.SDL.SDL_free(buttons[(System.Int32)(i)].text);
        ''' 			}
        ''' 			SDL2.SDL.SDL_free(data.message);
        ''' 			SDL2.SDL.SDL_free(data.title);
        ''' 
        ''' 			return result;
        ''' 		}
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* window refers to an SDL_Window* */
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_ShowSimpleMessageBox", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe int INTERNAL_SDL_ShowSimpleMessageBox(
        ''' 			SDL2.SDL.SDL_MessageBoxFlags flags,
        ''' 			byte* title,
        ''' 			byte* message,
        ''' 			System.IntPtr window
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe int SDL_ShowSimpleMessageBox(
        ''' 			SDL2.SDL.SDL_MessageBoxFlags flags,
        ''' 			string title,
        ''' 			string message,
        ''' 			System.IntPtr window
        ''' 		)
        ''' 		{
        ''' 			int utf8TitleBufSize = SDL2.SDL.Utf8SizeNullable(title);
        ''' 			byte* utf8Title = stackalloc byte[utf8TitleBufSize];
        ''' 
        ''' 			int utf8MessageBufSize = SDL2.SDL.Utf8SizeNullable(message);
        ''' 			byte* utf8Message = stackalloc byte[utf8MessageBufSize];
        ''' 
        ''' 			return SDL2.SDL.INTERNAL_SDL_ShowSimpleMessageBox(
        ''' 				flags,
        ''' 				SDL2.SDL.Utf8EncodeNullable(title, utf8Title, utf8TitleBufSize),
        ''' 				SDL2.SDL.Utf8EncodeNullable(message, utf8Message, utf8MessageBufSize),
        ''' 				window
        ''' 			);
        ''' 		}
        ''' 
        ''' 

#End Region

#Region "SDL_version.h, SDL_revision.h"

        ' Similar to the headers, this is the version we're expecting to be
        ' 		 * running with. You will likely want to check this somewhere in your
        ' 		 * program!
        ' 		 
        Public Const SDL_MAJOR_VERSION As Integer = 2
        Public Const SDL_MINOR_VERSION As Integer = 0
        Public Const SDL_PATCHLEVEL As Integer = 12
        Public ReadOnly SDL_COMPILEDVERSION As Integer = SDL_VERSIONNUM(SDL_MAJOR_VERSION, SDL_MINOR_VERSION, SDL_PATCHLEVEL)

        <StructLayout(LayoutKind.Sequential)>
        Public Structure n_SDL_version
            Public major As Byte
            Public minor As Byte
            Public patch As Byte
        End Structure

        Public Sub SDL_VERSION(<Out> ByRef x As n_SDL_version)
            x.major = SDL_MAJOR_VERSION
            x.minor = SDL_MINOR_VERSION
            x.patch = SDL_PATCHLEVEL
        End Sub

        Public Function SDL_VERSIONNUM(ByVal X As Integer, ByVal Y As Integer, ByVal Z As Integer) As Integer
            Return X * 1000 + Y * 100 + Z
        End Function

        Public Function SDL_VERSION_ATLEAST(ByVal X As Integer, ByVal Y As Integer, ByVal Z As Integer) As Boolean
            Return SDL_COMPILEDVERSION >= SDL_VERSIONNUM(X, Y, Z)
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_GetVersion(<Out> ByRef ver As n_SDL_version)
        End Sub

        <DllImport(nativeLibName, EntryPoint:="SDL_GetRevision", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GetRevision() As IntPtr
        End Function

        Public Function SDL_GetRevision() As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GetRevision())
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetRevisionNumber() As Integer
        End Function


#End Region

#Region "SDL_video.h"

        Public Enum SDL_GLattr
            SDL_GL_RED_SIZE
            SDL_GL_GREEN_SIZE
            SDL_GL_BLUE_SIZE
            SDL_GL_ALPHA_SIZE
            SDL_GL_BUFFER_SIZE
            SDL_GL_DOUBLEBUFFER
            SDL_GL_DEPTH_SIZE
            SDL_GL_STENCIL_SIZE
            SDL_GL_ACCUM_RED_SIZE
            SDL_GL_ACCUM_GREEN_SIZE
            SDL_GL_ACCUM_BLUE_SIZE
            SDL_GL_ACCUM_ALPHA_SIZE
            SDL_GL_STEREO
            SDL_GL_MULTISAMPLEBUFFERS
            SDL_GL_MULTISAMPLESAMPLES
            SDL_GL_ACCELERATED_VISUAL
            SDL_GL_RETAINED_BACKING
            SDL_GL_CONTEXT_MAJOR_VERSION
            SDL_GL_CONTEXT_MINOR_VERSION
            SDL_GL_CONTEXT_EGL
            SDL_GL_CONTEXT_FLAGS
            SDL_GL_CONTEXT_PROFILE_MASK
            SDL_GL_SHARE_WITH_CURRENT_CONTEXT
            SDL_GL_FRAMEBUFFER_SRGB_CAPABLE
            SDL_GL_CONTEXT_RELEASE_BEHAVIOR
            SDL_GL_CONTEXT_RESET_NOTIFICATION  ' Requires >= 2.0.6 
            SDL_GL_CONTEXT_NO_ERROR        ' Requires >= 2.0.6 
        End Enum

        <Flags>
        Public Enum SDL_GLprofile
            SDL_GL_CONTEXT_PROFILE_CORE = &H1
            SDL_GL_CONTEXT_PROFILE_COMPATIBILITY = &H2
            SDL_GL_CONTEXT_PROFILE_ES = &H4
        End Enum

        <Flags>
        Public Enum SDL_GLcontext
            SDL_GL_CONTEXT_DEBUG_FLAG = &H1
            SDL_GL_CONTEXT_FORWARD_COMPATIBLE_FLAG = &H2
            SDL_GL_CONTEXT_ROBUST_ACCESS_FLAG = &H4
            SDL_GL_CONTEXT_RESET_ISOLATION_FLAG = &H8
        End Enum

        Public Enum SDL_WindowEventID As Byte
            SDL_WINDOWEVENT_NONE
            SDL_WINDOWEVENT_SHOWN
            SDL_WINDOWEVENT_HIDDEN
            SDL_WINDOWEVENT_EXPOSED
            SDL_WINDOWEVENT_MOVED
            SDL_WINDOWEVENT_RESIZED
            SDL_WINDOWEVENT_SIZE_CHANGED
            SDL_WINDOWEVENT_MINIMIZED
            SDL_WINDOWEVENT_MAXIMIZED
            SDL_WINDOWEVENT_RESTORED
            SDL_WINDOWEVENT_ENTER
            SDL_WINDOWEVENT_LEAVE
            SDL_WINDOWEVENT_FOCUS_GAINED
            SDL_WINDOWEVENT_FOCUS_LOST
            SDL_WINDOWEVENT_CLOSE
            ' Only available in 2.0.5 or higher. 
            SDL_WINDOWEVENT_TAKE_FOCUS
            SDL_WINDOWEVENT_HIT_TEST
        End Enum

        Public Enum SDL_DisplayEventID As Byte
            SDL_DISPLAYEVENT_NONE
            SDL_DISPLAYEVENT_ORIENTATION
        End Enum

        Public Enum SDL_DisplayOrientation
            SDL_ORIENTATION_UNKNOWN
            SDL_ORIENTATION_LANDSCAPE
            SDL_ORIENTATION_LANDSCAPE_FLIPPED
            SDL_ORIENTATION_PORTRAIT
            SDL_ORIENTATION_PORTRAIT_FLIPPED
        End Enum

        <Flags>
        Public Enum SDL_WindowFlags As UInteger
            SDL_WINDOW_FULLSCREEN = &H1
            SDL_WINDOW_OPENGL = &H2
            SDL_WINDOW_SHOWN = &H4
            SDL_WINDOW_HIDDEN = &H8
            SDL_WINDOW_BORDERLESS = &H10
            SDL_WINDOW_RESIZABLE = &H20
            SDL_WINDOW_MINIMIZED = &H40
            SDL_WINDOW_MAXIMIZED = &H80
            SDL_WINDOW_INPUT_GRABBED = &H100
            SDL_WINDOW_INPUT_FOCUS = &H200
            SDL_WINDOW_MOUSE_FOCUS = &H400
            SDL_WINDOW_FULLSCREEN_DESKTOP = SDL_WINDOW_FULLSCREEN Or &H1000
            SDL_WINDOW_FOREIGN = &H800
            SDL_WINDOW_ALLOW_HIGHDPI = &H2000  ' Requires >= 2.0.1 
            SDL_WINDOW_MOUSE_CAPTURE = &H4000  ' Requires >= 2.0.4 
            SDL_WINDOW_ALWAYS_ON_TOP = &H8000  ' Requires >= 2.0.5 
            SDL_WINDOW_SKIP_TASKBAR = &H10000   ' Requires >= 2.0.5 
            SDL_WINDOW_UTILITY = &H20000    ' Requires >= 2.0.5 
            SDL_WINDOW_TOOLTIP = &H40000    ' Requires >= 2.0.5 
            SDL_WINDOW_POPUP_MENU = &H80000 ' Requires >= 2.0.5 
            SDL_WINDOW_VULKAN = &H10000000 ' Requires >= 2.0.6 
        End Enum


        ' Only available in 2.0.4 or higher. 
        Public Enum SDL_HitTestResult
            SDL_HITTEST_NORMAL     ' Region is normal. No special properties. 
            SDL_HITTEST_DRAGGABLE      ' Region can drag entire window. 
            SDL_HITTEST_RESIZE_TOPLEFT
            SDL_HITTEST_RESIZE_TOP
            SDL_HITTEST_RESIZE_TOPRIGHT
            SDL_HITTEST_RESIZE_RIGHT
            SDL_HITTEST_RESIZE_BOTTOMRIGHT
            SDL_HITTEST_RESIZE_BOTTOM
            SDL_HITTEST_RESIZE_BOTTOMLEFT
            SDL_HITTEST_RESIZE_LEFT
        End Enum

        Public Const SDL_WINDOWPOS_UNDEFINED_MASK As Integer = &H1FFF0000
        Public Const SDL_WINDOWPOS_CENTERED_MASK As Integer = &H2FFF0000
        Public Const SDL_WINDOWPOS_UNDEFINED As Integer = &H1FFF0000
        Public Const SDL_WINDOWPOS_CENTERED As Integer = &H2FFF0000

        Public Function SDL_WINDOWPOS_UNDEFINED_DISPLAY(ByVal X As Integer) As Integer
            Return SDL_WINDOWPOS_UNDEFINED_MASK Or X
        End Function

        Public Function SDL_WINDOWPOS_ISUNDEFINED(ByVal X As Integer) As Boolean
            Return (X And &HFFFF0000) = SDL_WINDOWPOS_UNDEFINED_MASK
        End Function

        Public Function SDL_WINDOWPOS_CENTERED_DISPLAY(ByVal X As Integer) As Integer
            Return SDL_WINDOWPOS_CENTERED_MASK Or X
        End Function

        Public Function SDL_WINDOWPOS_ISCENTERED(ByVal X As Integer) As Boolean
            Return (X And &HFFFF0000) = SDL_WINDOWPOS_CENTERED_MASK
        End Function

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_DisplayMode
            Public format As UInteger
            Public w As Integer
            Public h As Integer
            Public refresh_rate As Integer
            Public driverdata As IntPtr ' void*
        End Structure


        ' win refers to an SDL_Window*, area to a const SDL_Point*, data to a void*.
        ' 		 * Only available in 2.0.4 or higher.
        ' 		 
        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function SDL_HitTest(ByVal win As IntPtr, ByVal area As IntPtr, ByVal data As IntPtr) As SDL_HitTestResult
        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* IntPtr refers to an SDL_Window* */
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_CreateWindow", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe System.IntPtr INTERNAL_SDL_CreateWindow(
        ''' 			byte* title,
        ''' 			int x,
        ''' 			int y,
        ''' 			int w,
        ''' 			int h,
        ''' 			SDL2.SDL.SDL_WindowFlags flags
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe System.IntPtr SDL_CreateWindow(
        ''' 			string title,
        ''' 			int x,
        ''' 			int y,
        ''' 			int w,
        ''' 			int h,
        ''' 			SDL2.SDL.SDL_WindowFlags flags
        ''' 		)
        ''' 		{
        ''' 			int utf8TitleBufSize = SDL2.SDL.Utf8SizeNullable(title);
        ''' 			byte* utf8Title = stackalloc byte[utf8TitleBufSize];
        ''' 			return SDL2.SDL.INTERNAL_SDL_CreateWindow(
        ''' 				SDL2.SDL.Utf8EncodeNullable(title, utf8Title, utf8TitleBufSize),
        ''' 				x, y, w, h,
        ''' 				flags
        ''' 			);
        ''' 		}
        ''' 
        ''' 
        <DllImport(nativeLibName, EntryPoint:="SDL_CreateWindow", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_CreateWindow(title() As Byte, x As Integer, y As Integer, width As Integer, height As Integer, flags As SDL_WindowFlags) As IntPtr
        End Function
        Public Function SDL_CreateWindow(title As String, x As Integer, y As Integer, width As Integer, height As Integer, flags As SDL_WindowFlags) As IntPtr
            Return INTERNAL_SDL_CreateWindow(UTF8_ToNative(title), x, y, width, height, flags)
        End Function

        ' window refers to an SDL_Window*, renderer to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_CreateWindowAndRenderer(ByVal width As Integer, ByVal height As Integer, ByVal window_flags As SDL_WindowFlags, <Out> ByRef window As IntPtr, <Out> ByRef renderer As IntPtr) As Integer
        End Function


        ' data refers to some native window type, IntPtr to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_CreateWindowFrom(ByVal data As IntPtr) As IntPtr
        End Function


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_DestroyWindow(ByVal window As IntPtr)
        End Sub

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_DisableScreenSaver()
        End Sub

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_EnableScreenSaver()
        End Sub


        ' IntPtr refers to an SDL_DisplayMode. Just use closest. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetClosestDisplayMode(ByVal displayIndex As Integer, ByRef mode As SDL_DisplayMode, <Out> ByRef closest As SDL_DisplayMode) As IntPtr
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetCurrentDisplayMode(ByVal displayIndex As Integer, <Out> ByRef mode As SDL_DisplayMode) As Integer
        End Function

        <DllImport(nativeLibName, EntryPoint:="SDL_GetCurrentVideoDriver", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GetCurrentVideoDriver() As IntPtr
        End Function

        Public Function SDL_GetCurrentVideoDriver() As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GetCurrentVideoDriver())
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetDesktopDisplayMode(ByVal displayIndex As Integer, <Out> ByRef mode As SDL_DisplayMode) As Integer
        End Function

        <DllImport(nativeLibName, EntryPoint:="SDL_GetDisplayName", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GetDisplayName(ByVal index As Integer) As IntPtr
        End Function

        Public Function SDL_GetDisplayName(ByVal index As Integer) As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GetDisplayName(index))
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetDisplayBounds(ByVal displayIndex As Integer, <Out> ByRef rect As SDL_Rect) As Integer
        End Function


        ' Only available in 2.0.4 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetDisplayDPI(ByVal displayIndex As Integer, <Out> ByRef ddpi As Single, <Out> ByRef hdpi As Single, <Out> ByRef vdpi As Single) As Integer
        End Function


        ' Only available in 2.0.9 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetDisplayOrientation(ByVal displayIndex As Integer) As SDL_DisplayOrientation
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetDisplayMode(ByVal displayIndex As Integer, ByVal modeIndex As Integer, <Out> ByRef mode As SDL_DisplayMode) As Integer
        End Function


        ' Only available in 2.0.5 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetDisplayUsableBounds(ByVal displayIndex As Integer, <Out> ByRef rect As SDL_Rect) As Integer
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetNumDisplayModes(ByVal displayIndex As Integer) As Integer
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetNumVideoDisplays() As Integer
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetNumVideoDrivers() As Integer
        End Function

        <DllImport(nativeLibName, EntryPoint:="SDL_GetVideoDriver", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GetVideoDriver(ByVal index As Integer) As IntPtr
        End Function

        Public Function SDL_GetVideoDriver(ByVal index As Integer) As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GetVideoDriver(index))
        End Function


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetWindowBrightness(ByVal window As IntPtr) As Single
        End Function


        ' window refers to an SDL_Window*
        ' 		 * Only available in 2.0.5 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetWindowOpacity(ByVal window As IntPtr, ByVal opacity As Single) As Integer
        End Function


        ' window refers to an SDL_Window*
        ' 		 * Only available in 2.0.5 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetWindowOpacity(ByVal window As IntPtr, <Out> ByRef out_opacity As Single) As Integer
        End Function


        ' modal_window and parent_window refer to an SDL_Window*s
        ' 		 * Only available in 2.0.5 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetWindowModalFor(ByVal modal_window As IntPtr, ByVal parent_window As IntPtr) As Integer
        End Function


        ' window refers to an SDL_Window*
        ' 		 * Only available in 2.0.5 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetWindowInputFocus(ByVal window As IntPtr) As Integer
        End Function

        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* window refers to an SDL_Window*, IntPtr to a void* */
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_GetWindowData", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe System.IntPtr INTERNAL_SDL_GetWindowData(
        ''' 			System.IntPtr window,
        ''' 			byte* name
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe System.IntPtr SDL_GetWindowData(
        ''' 			System.IntPtr window,
        ''' 			string name
        ''' 		)
        ''' 		{
        ''' 			int utf8NameBufSize = SDL2.SDL.Utf8Size(name);
        ''' 			byte* utf8Name = stackalloc byte[utf8NameBufSize];
        ''' 			return SDL2.SDL.INTERNAL_SDL_GetWindowData(
        ''' 				window,
        ''' 				SDL2.SDL.Utf8Encode(name, utf8Name, utf8NameBufSize)
        ''' 			);
        ''' 		}
        ''' 
        ''' 

        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetWindowDisplayIndex(ByVal window As IntPtr) As Integer
        End Function


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetWindowDisplayMode(ByVal window As IntPtr, <Out> ByRef mode As SDL_DisplayMode) As Integer
        End Function


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetWindowFlags(ByVal window As IntPtr) As UInteger
        End Function


        ' IntPtr refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetWindowFromID(ByVal id As UInteger) As IntPtr
        End Function


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetWindowGammaRamp(ByVal window As IntPtr,
        <Out()>
        <MarshalAs(UnmanagedType.LPArray, ArraySubType:=UnmanagedType.U2, SizeConst:=256)> ByVal red As UShort(),
        <Out()>
        <MarshalAs(UnmanagedType.LPArray, ArraySubType:=UnmanagedType.U2, SizeConst:=256)> ByVal green As UShort(),
        <Out()>
        <MarshalAs(UnmanagedType.LPArray, ArraySubType:=UnmanagedType.U2, SizeConst:=256)> ByVal blue As UShort()) As Integer
        End Function


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetWindowGrab(ByVal window As IntPtr) As SDL_bool
        End Function


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetWindowID(ByVal window As IntPtr) As UInteger
        End Function


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetWindowPixelFormat(ByVal window As IntPtr) As UInteger
        End Function


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_GetWindowMaximumSize(ByVal window As IntPtr, <Out> ByRef max_w As Integer, <Out> ByRef max_h As Integer)
        End Sub


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_GetWindowMinimumSize(ByVal window As IntPtr, <Out> ByRef min_w As Integer, <Out> ByRef min_h As Integer)
        End Sub


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_GetWindowPosition(ByVal window As IntPtr, <Out> ByRef x As Integer, <Out> ByRef y As Integer)
        End Sub


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_GetWindowSize(ByVal window As IntPtr, <Out> ByRef w As Integer, <Out> ByRef h As Integer)
        End Sub


        ' IntPtr refers to an SDL_Surface*, window to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetWindowSurface(ByVal window As IntPtr) As IntPtr
        End Function


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, EntryPoint:="SDL_GetWindowTitle", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GetWindowTitle(ByVal window As IntPtr) As IntPtr
        End Function

        Public Function SDL_GetWindowTitle(ByVal window As IntPtr) As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GetWindowTitle(window))
        End Function


        ' texture refers to an SDL_Texture* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GL_BindTexture(ByVal texture As IntPtr, <Out> ByRef texw As Single, <Out> ByRef texh As Single) As Integer
        End Function


        ' IntPtr and window refer to an SDL_GLContext and SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GL_CreateContext(ByVal window As IntPtr) As IntPtr
        End Function


        ' context refers to an SDL_GLContext 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_GL_DeleteContext(ByVal context As IntPtr)
        End Sub

        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_GL_LoadLibrary", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe int INTERNAL_SDL_GL_LoadLibrary(byte* path);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe int SDL_GL_LoadLibrary(string path)
        ''' 		{
        ''' 			byte* utf8Path = SDL2.SDL.Utf8Encode(path);
        ''' 			int result = SDL2.SDL.INTERNAL_SDL_GL_LoadLibrary(
        ''' 				utf8Path
        ''' 			);
        ''' 			System.Runtime.InteropServices.Marshal.FreeHGlobal((System.IntPtr)utf8Path);
        ''' 			return result;
        ''' 		}
        ''' 
        ''' 

        ' IntPtr refers to a function pointer, proc to a const char* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GL_GetProcAddress(ByVal proc As IntPtr) As IntPtr
        End Function

        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* IntPtr refers to a function pointer */
        ''' 		public static unsafe System.IntPtr SDL_GL_GetProcAddress(string proc)
        ''' 		{
        ''' 			int utf8ProcBufSize = SDL2.SDL.Utf8Size(proc);
        ''' 			byte* utf8Proc = stackalloc byte[utf8ProcBufSize];
        ''' 			return SDL2.SDL.SDL_GL_GetProcAddress(
        ''' 				(System.IntPtr)SDL2.SDL.Utf8Encode(proc, utf8Proc, utf8ProcBufSize)
        ''' 			);
        ''' 		}
        ''' 
        ''' 

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_GL_UnloadLibrary()
        End Sub

        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_GL_ExtensionSupported", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe SDL2.SDL.SDL_bool INTERNAL_SDL_GL_ExtensionSupported(
        ''' 			byte* extension
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe SDL2.SDL.SDL_bool SDL_GL_ExtensionSupported(string extension)
        ''' 		{
        ''' 			int utf8ExtensionBufSize = SDL2.SDL.Utf8SizeNullable(extension);
        ''' 			byte* utf8Extension = stackalloc byte[utf8ExtensionBufSize];
        ''' 			return SDL2.SDL.INTERNAL_SDL_GL_ExtensionSupported(
        ''' 				SDL2.SDL.Utf8Encode(extension, utf8Extension, utf8ExtensionBufSize)
        ''' 			);
        ''' 		}
        ''' 
        ''' 

        ' Only available in SDL 2.0.2 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_GL_ResetAttributes()
        End Sub

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GL_GetAttribute(ByVal attr As SDL_GLattr, <Out> ByRef value As Integer) As Integer
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GL_GetSwapInterval() As Integer
        End Function


        ' window and context refer to an SDL_Window* and SDL_GLContext 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GL_MakeCurrent(ByVal window As IntPtr, ByVal context As IntPtr) As Integer
        End Function


        ' IntPtr refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GL_GetCurrentWindow() As IntPtr
        End Function


        ' IntPtr refers to an SDL_Context 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GL_GetCurrentContext() As IntPtr
        End Function


        ' window refers to an SDL_Window*.
        ' 		 * Only available in SDL 2.0.1 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_GL_GetDrawableSize(ByVal window As IntPtr, <Out> ByRef w As Integer, <Out> ByRef h As Integer)
        End Sub

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GL_SetAttribute(ByVal attr As SDL_GLattr, ByVal value As Integer) As Integer
        End Function

        Public Function SDL_GL_SetAttribute(ByVal attr As SDL_GLattr, ByVal profile As SDL_GLprofile) As Integer
            Return SDL_GL_SetAttribute(attr, CInt(profile))
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GL_SetSwapInterval(ByVal interval As Integer) As Integer
        End Function


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_GL_SwapWindow(ByVal window As IntPtr)
        End Sub


        ' texture refers to an SDL_Texture* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GL_UnbindTexture(ByVal texture As IntPtr) As Integer
        End Function


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_HideWindow(ByVal window As IntPtr)
        End Sub

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_IsScreenSaverEnabled() As SDL_bool
        End Function


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_MaximizeWindow(ByVal window As IntPtr)
        End Sub


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_MinimizeWindow(ByVal window As IntPtr)
        End Sub


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_RaiseWindow(ByVal window As IntPtr)
        End Sub


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_RestoreWindow(ByVal window As IntPtr)
        End Sub


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetWindowBrightness(ByVal window As IntPtr, ByVal brightness As Single) As Integer
        End Function

        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* IntPtr and userdata are void*, window is an SDL_Window* */
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_SetWindowData", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe System.IntPtr INTERNAL_SDL_SetWindowData(
        ''' 			System.IntPtr window,
        ''' 			byte* name,
        ''' 			System.IntPtr userdata
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe System.IntPtr SDL_SetWindowData(
        ''' 			System.IntPtr window,
        ''' 			string name,
        ''' 			System.IntPtr userdata
        ''' 		)
        ''' 		{
        ''' 			int utf8NameBufSize = SDL2.SDL.Utf8Size(name);
        ''' 			byte* utf8Name = stackalloc byte[utf8NameBufSize];
        ''' 			return SDL2.SDL.INTERNAL_SDL_SetWindowData(
        ''' 				window,
        ''' 				SDL2.SDL.Utf8Encode(name, utf8Name, utf8NameBufSize),
        ''' 				userdata
        ''' 			);
        ''' 		}
        ''' 
        ''' 

        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetWindowDisplayMode(ByVal window As IntPtr, ByRef mode As SDL_DisplayMode) As Integer
        End Function


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetWindowFullscreen(ByVal window As IntPtr, ByVal flags As UInteger) As Integer
        End Function


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetWindowGammaRamp(ByVal window As IntPtr,
        <[In]()>
        <MarshalAs(UnmanagedType.LPArray, ArraySubType:=UnmanagedType.U2, SizeConst:=256)> ByVal red As UShort(),
        <[In]()>
        <MarshalAs(UnmanagedType.LPArray, ArraySubType:=UnmanagedType.U2, SizeConst:=256)> ByVal green As UShort(),
        <[In]()>
        <MarshalAs(UnmanagedType.LPArray, ArraySubType:=UnmanagedType.U2, SizeConst:=256)> ByVal blue As UShort()) As Integer
        End Function


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_SetWindowGrab(ByVal window As IntPtr, ByVal grabbed As SDL_bool)
        End Sub


        ' window refers to an SDL_Window*, icon to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_SetWindowIcon(ByVal window As IntPtr, ByVal icon As IntPtr)
        End Sub


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_SetWindowMaximumSize(ByVal window As IntPtr, ByVal max_w As Integer, ByVal max_h As Integer)
        End Sub


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_SetWindowMinimumSize(ByVal window As IntPtr, ByVal min_w As Integer, ByVal min_h As Integer)
        End Sub


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_SetWindowPosition(ByVal window As IntPtr, ByVal x As Integer, ByVal y As Integer)
        End Sub


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_SetWindowSize(ByVal window As IntPtr, ByVal w As Integer, ByVal h As Integer)
        End Sub


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_SetWindowBordered(ByVal window As IntPtr, ByVal bordered As SDL_bool)
        End Sub


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetWindowBordersSize(ByVal window As IntPtr, <Out> ByRef top As Integer, <Out> ByRef left As Integer, <Out> ByRef bottom As Integer, <Out> ByRef right As Integer) As Integer
        End Function


        ' window refers to an SDL_Window*
        ' 		 * Only available in 2.0.5 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_SetWindowResizable(ByVal window As IntPtr, ByVal resizable As SDL_bool)
        End Sub

        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* window refers to an SDL_Window* */
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_SetWindowTitle", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe void INTERNAL_SDL_SetWindowTitle(
        ''' 			System.IntPtr window,
        ''' 			byte* title
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe void SDL_SetWindowTitle(
        ''' 			System.IntPtr window,
        ''' 			string title
        ''' 		)
        ''' 		{
        ''' 			int utf8TitleBufSize = SDL2.SDL.Utf8Size(title);
        ''' 			byte* utf8Title = stackalloc byte[utf8TitleBufSize];
        ''' 			SDL2.SDL.INTERNAL_SDL_SetWindowTitle(
        ''' 				window,
        ''' 				SDL2.SDL.Utf8Encode(title, utf8Title, utf8TitleBufSize)
        ''' 			);
        ''' 		}
        ''' 
        ''' 

        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_ShowWindow(ByVal window As IntPtr)
        End Sub


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_UpdateWindowSurface(ByVal window As IntPtr) As Integer
        End Function


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_UpdateWindowSurfaceRects(ByVal window As IntPtr,
        <[In]> ByVal rects As SDL_Rect(), ByVal numrects As Integer) As Integer
        End Function

        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_VideoInit", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe int INTERNAL_SDL_VideoInit(
        ''' 			byte* driver_name
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe int SDL_VideoInit(string driver_name)
        ''' 		{
        ''' 			int utf8DriverNameBufSize = SDL2.SDL.Utf8Size(driver_name);
        ''' 			byte* utf8DriverName = stackalloc byte[utf8DriverNameBufSize];
        ''' 			return SDL2.SDL.INTERNAL_SDL_VideoInit(
        ''' 				SDL2.SDL.Utf8Encode(driver_name, utf8DriverName, utf8DriverNameBufSize)
        ''' 			);
        ''' 		}
        ''' 
        ''' 

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_VideoQuit()
        End Sub


        ' window refers to an SDL_Window*, callback_data to a void*
        ' 		 * Only available in 2.0.4 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetWindowHitTest(ByVal window As IntPtr, ByVal callback As SDL_HitTest, ByVal callback_data As IntPtr) As Integer
        End Function


        ' IntPtr refers to an SDL_Window*
        ' 		 * Only available in 2.0.4 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetGrabbedWindow() As IntPtr
        End Function


#End Region

#Region "SDL_blendmode.h"

        <Flags>
        Public Enum SDL_BlendMode
            SDL_BLENDMODE_NONE = &H0
            SDL_BLENDMODE_BLEND = &H1
            SDL_BLENDMODE_ADD = &H2
            SDL_BLENDMODE_MOD = &H4
            SDL_BLENDMODE_MUL = &H8 ' >= 2.0.11 
            SDL_BLENDMODE_INVALID = &H7FFFFFFF
        End Enum

        Public Enum SDL_BlendOperation
            SDL_BLENDOPERATION_ADD = &H1
            SDL_BLENDOPERATION_SUBTRACT = &H2
            SDL_BLENDOPERATION_REV_SUBTRACT = &H3
            SDL_BLENDOPERATION_MINIMUM = &H4
            SDL_BLENDOPERATION_MAXIMUM = &H5
        End Enum

        Public Enum SDL_BlendFactor
            SDL_BLENDFACTOR_ZERO = &H1
            SDL_BLENDFACTOR_ONE = &H2
            SDL_BLENDFACTOR_SRC_COLOR = &H3
            SDL_BLENDFACTOR_ONE_MINUS_SRC_COLOR = &H4
            SDL_BLENDFACTOR_SRC_ALPHA = &H5
            SDL_BLENDFACTOR_ONE_MINUS_SRC_ALPHA = &H6
            SDL_BLENDFACTOR_DST_COLOR = &H7
            SDL_BLENDFACTOR_ONE_MINUS_DST_COLOR = &H8
            SDL_BLENDFACTOR_DST_ALPHA = &H9
            SDL_BLENDFACTOR_ONE_MINUS_DST_ALPHA = &HA
        End Enum



#End Region

#Region "SDL_vulkan.h"

        ' Only available in 2.0.6 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_ComposeCustomBlendMode(ByVal srcColorFactor As SDL_BlendFactor, ByVal dstColorFactor As SDL_BlendFactor, ByVal colorOperation As SDL_BlendOperation, ByVal srcAlphaFactor As SDL_BlendFactor, ByVal dstAlphaFactor As SDL_BlendFactor, ByVal alphaOperation As SDL_BlendOperation) As SDL_BlendMode
        End Function

        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		#endregion
        ''' 
        ''' 		#region SDL_vulkan.h
        ''' 
        ''' 		/* Only available in 2.0.6 or higher. */
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_Vulkan_LoadLibrary", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe int INTERNAL_SDL_Vulkan_LoadLibrary(
        ''' 			byte* path
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe int SDL_Vulkan_LoadLibrary(string path)
        ''' 		{
        ''' 			byte* utf8Path = SDL2.SDL.Utf8Encode(path);
        ''' 			int result = SDL2.SDL.INTERNAL_SDL_Vulkan_LoadLibrary(
        ''' 				utf8Path
        ''' 			);
        ''' 			System.Runtime.InteropServices.Marshal.FreeHGlobal((System.IntPtr)utf8Path);
        ''' 			return result;
        ''' 		}
        ''' 
        ''' 

        ' Only available in 2.0.6 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_Vulkan_GetVkGetInstanceProcAddr() As IntPtr
        End Function


        ' Only available in 2.0.6 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_Vulkan_UnloadLibrary()
        End Sub


        ' window refers to an SDL_Window*, pNames to a const char**.
        ' 		 * Only available in 2.0.6 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_Vulkan_GetInstanceExtensions(ByVal window As IntPtr, <Out> ByRef pCount As UInteger, ByVal pNames As IntPtr()) As SDL_bool
        End Function


        ' window refers to an SDL_Window.
        ' 		 * instance refers to a VkInstance.
        ' 		 * surface refers to a VkSurfaceKHR.
        ' 		 * Only available in 2.0.6 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_Vulkan_CreateSurface(ByVal window As IntPtr, ByVal instance As IntPtr, <Out> ByRef surface As ULong) As SDL_bool
        End Function


        ' window refers to an SDL_Window*.
        ' 		 * Only available in 2.0.6 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_Vulkan_GetDrawableSize(ByVal window As IntPtr, <Out> ByRef w As Integer, <Out> ByRef h As Integer)
        End Sub


#End Region

#Region "SDL_metal.h"

        ' Only available in 2.0.11 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_Metal_CreateView(ByVal window As IntPtr) As IntPtr
        End Function


        ' Only available in 2.0.11 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_Metal_DestroyView(ByVal view As IntPtr)
        End Sub


#End Region

#Region "SDL_render.h"

        <Flags>
        Public Enum SDL_RendererFlags As UInteger
            SDL_RENDERER_SOFTWARE = &H1
            SDL_RENDERER_ACCELERATED = &H2
            SDL_RENDERER_PRESENTVSYNC = &H4
            SDL_RENDERER_TARGETTEXTURE = &H8
        End Enum

        <Flags>
        Public Enum SDL_RendererFlip
            SDL_FLIP_NONE = &H0
            SDL_FLIP_HORIZONTAL = &H1
            SDL_FLIP_VERTICAL = &H2
        End Enum

        Public Enum SDL_TextureAccess
            SDL_TEXTUREACCESS_STATIC
            SDL_TEXTUREACCESS_STREAMING
            SDL_TEXTUREACCESS_TARGET
        End Enum

        <Flags>
        Public Enum SDL_TextureModulate
            SDL_TEXTUREMODULATE_NONE = &H0
            SDL_TEXTUREMODULATE_HORIZONTAL = &H1
            SDL_TEXTUREMODULATE_VERTICAL = &H2
        End Enum ' const char*
        ''' Cannot convert StructDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitStructDeclaration(StructDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		[System.Runtime.InteropServices.@StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        ''' 		public unsafe struct SDL_RendererInfo
        ''' 		{
        ''' 			public System.IntPtr name; // const char*
        ''' 			public uint flags;
        ''' 			public uint num_texture_formats;
        ''' 			public fixed uint texture_formats[16];
        ''' 			public int max_texture_width;
        ''' 			public int max_texture_height;
        ''' 		}
        ''' 
        ''' 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_RendererInfo
            Public name As IntPtr
            Public flags As UInteger
            Public num_texture_formats As UInteger
            <VBFixedArray(16)> Public texture_formats() As UInteger 'fixed length array
            Public max_texture_width As Integer
            Public max_texture_height As Integer
        End Structure

        ' Only available in 2.0.11 or higher. 
        Public Enum SDL_ScaleMode
            SDL_ScaleModeNearest
            SDL_ScaleModeLinear
            SDL_ScaleModeBest
        End Enum


        ' IntPtr refers to an SDL_Renderer*, window to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_CreateRenderer(ByVal window As IntPtr, ByVal index As Integer, ByVal flags As SDL_RendererFlags) As IntPtr
        End Function


        ' IntPtr refers to an SDL_Renderer*, surface to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_CreateSoftwareRenderer(ByVal surface As IntPtr) As IntPtr
        End Function


        ' IntPtr refers to an SDL_Texture*, renderer to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_CreateTexture(ByVal renderer As IntPtr, ByVal format As UInteger, ByVal access As Integer, ByVal w As Integer, ByVal h As Integer) As IntPtr
        End Function


        ' IntPtr refers to an SDL_Texture*
        ' 		 * renderer refers to an SDL_Renderer*
        ' 		 * surface refers to an SDL_Surface*
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_CreateTextureFromSurface(ByVal renderer As IntPtr, ByVal surface As IntPtr) As IntPtr
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_DestroyRenderer(ByVal renderer As IntPtr)
        End Sub


        ' texture refers to an SDL_Texture* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_DestroyTexture(ByVal texture As IntPtr)
        End Sub

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetNumRenderDrivers() As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetRenderDrawBlendMode(ByVal renderer As IntPtr, <Out> ByRef blendMode As SDL_BlendMode) As Integer
        End Function


        ' texture refers to an SDL_Texture*
        ' 		 * Only available in 2.0.11 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetTextureScaleMode(ByVal texture As IntPtr, ByVal scaleMode As SDL_ScaleMode) As Integer
        End Function


        ' texture refers to an SDL_Texture*
        ' 		 * Only available in 2.0.11 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetTextureScaleMode(ByVal texture As IntPtr, <Out> ByRef scaleMode As SDL_ScaleMode) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetRenderDrawColor(ByVal renderer As IntPtr, <Out> ByRef r As Byte, <Out> ByRef g As Byte, <Out> ByRef b As Byte, <Out> ByRef a As Byte) As Integer
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetRenderDriverInfo(ByVal index As Integer, <Out> ByRef info As SDL.SDL_RendererInfo) As Integer
        End Function


        ' IntPtr refers to an SDL_Renderer*, window to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetRenderer(ByVal window As IntPtr) As IntPtr
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetRendererInfo(ByVal renderer As IntPtr, <Out> ByRef info As SDL.SDL_RendererInfo) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetRendererOutputSize(ByVal renderer As IntPtr, <Out> ByRef w As Integer, <Out> ByRef h As Integer) As Integer
        End Function


        ' texture refers to an SDL_Texture* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetTextureAlphaMod(ByVal texture As IntPtr, <Out> ByRef alpha As Byte) As Integer
        End Function


        ' texture refers to an SDL_Texture* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetTextureBlendMode(ByVal texture As IntPtr, <Out> ByRef blendMode As SDL_BlendMode) As Integer
        End Function


        ' texture refers to an SDL_Texture* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetTextureColorMod(ByVal texture As IntPtr, <Out> ByRef r As Byte, <Out> ByRef g As Byte, <Out> ByRef b As Byte) As Integer
        End Function


        ' texture refers to an SDL_Texture*, pixels to a void* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_LockTexture(ByVal texture As IntPtr, ByRef rect As SDL_Rect, <Out> ByRef pixels As IntPtr, <Out> ByRef pitch As Integer) As Integer
        End Function


        ' texture refers to an SDL_Texture*, pixels to a void*.
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * the rectangle is passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero to be passed for rect.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_LockTexture(ByVal texture As IntPtr, ByVal rect As IntPtr, <Out> ByRef pixels As IntPtr, <Out> ByRef pitch As Integer) As Integer
        End Function


        ' texture refers to an SDL_Texture*, surface to an SDL_Surface*
        ' 		 * Only available in 2.0.11 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_LockTextureToSurface(ByVal texture As IntPtr, ByRef rect As SDL_Rect, <Out> ByRef surface As IntPtr) As Integer
        End Function


        ' texture refers to an SDL_Texture*, surface to an SDL_Surface*
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * the rectangle is passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero to be passed for rect.
        ' 		 * Only available in 2.0.11 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_LockTextureToSurface(ByVal texture As IntPtr, ByVal rect As IntPtr, <Out> ByRef surface As IntPtr) As Integer
        End Function


        ' texture refers to an SDL_Texture* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_QueryTexture(ByVal texture As IntPtr, <Out> ByRef format As UInteger, <Out> ByRef access As Integer, <Out> ByRef w As Integer, <Out> ByRef h As Integer) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderClear(ByVal renderer As IntPtr) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopy(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByRef srcrect As SDL_Rect, ByRef dstrect As SDL_Rect) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source and destination rectangles are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for srcrect.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopy(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByVal srcrect As IntPtr, ByRef dstrect As SDL_Rect) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source and destination rectangles are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for dstrect.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopy(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByRef srcrect As SDL_Rect, ByVal dstrect As IntPtr) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source and destination rectangles are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for both SDL_Rects.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopy(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByVal srcrect As IntPtr, ByVal dstrect As IntPtr) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopyEx(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByRef srcrect As SDL_Rect, ByRef dstrect As SDL_Rect, ByVal angle As Double, ByRef center As SDL_Point, ByVal flip As SDL_RendererFlip) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source, destination, and/or center are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for srcrect.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopyEx(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByVal srcrect As IntPtr, ByRef dstrect As SDL_Rect, ByVal angle As Double, ByRef center As SDL_Point, ByVal flip As SDL_RendererFlip) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source, destination, and/or center are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for dstrect.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopyEx(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByRef srcrect As SDL_Rect, ByVal dstrect As IntPtr, ByVal angle As Double, ByRef center As SDL_Point, ByVal flip As SDL_RendererFlip) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source, destination, and/or center are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for center.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopyEx(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByRef srcrect As SDL_Rect, ByRef dstrect As SDL_Rect, ByVal angle As Double, ByVal center As IntPtr, ByVal flip As SDL_RendererFlip) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source, destination, and/or center are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for both
        ' 		 * srcrect and dstrect.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopyEx(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByVal srcrect As IntPtr, ByVal dstrect As IntPtr, ByVal angle As Double, ByRef center As SDL_Point, ByVal flip As SDL_RendererFlip) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source, destination, and/or center are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for both
        ' 		 * srcrect and center.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopyEx(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByVal srcrect As IntPtr, ByRef dstrect As SDL_Rect, ByVal angle As Double, ByVal center As IntPtr, ByVal flip As SDL_RendererFlip) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source, destination, and/or center are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for both
        ' 		 * dstrect and center.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopyEx(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByRef srcrect As SDL_Rect, ByVal dstrect As IntPtr, ByVal angle As Double, ByVal center As IntPtr, ByVal flip As SDL_RendererFlip) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source, destination, and/or center are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for all
        ' 		 * three parameters.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopyEx(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByVal srcrect As IntPtr, ByVal dstrect As IntPtr, ByVal angle As Double, ByVal center As IntPtr, ByVal flip As SDL_RendererFlip) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderDrawLine(ByVal renderer As IntPtr, ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderDrawLines(ByVal renderer As IntPtr,
        <[In]> ByVal points As SDL_Point(), ByVal count As Integer) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderDrawPoint(ByVal renderer As IntPtr, ByVal x As Integer, ByVal y As Integer) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderDrawPoints(ByVal renderer As IntPtr,
        <[In]> ByVal points As SDL_Point(), ByVal count As Integer) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderDrawRect(ByVal renderer As IntPtr, ByRef rect As SDL_Rect) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, rect to an SDL_Rect*.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for rect.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderDrawRect(ByVal renderer As IntPtr, ByVal rect As IntPtr) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderDrawRects(ByVal renderer As IntPtr,
        <[In]> ByVal rects As SDL_Rect(), ByVal count As Integer) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderFillRect(ByVal renderer As IntPtr, ByRef rect As SDL_Rect) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, rect to an SDL_Rect*.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for rect.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderFillRect(ByVal renderer As IntPtr, ByVal rect As IntPtr) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderFillRects(ByVal renderer As IntPtr,
        <[In]> ByVal rects As SDL_Rect(), ByVal count As Integer) As Integer
        End Function


#Region "Floating Point Render Functions"

        ' This region only available in SDL 2.0.10 or higher. 

        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopyF(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByRef srcrect As SDL_Rect, ByRef dstrect As SDL_FRect) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source and destination rectangles are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for srcrect.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopyF(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByVal srcrect As IntPtr, ByRef dstrect As SDL_FRect) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source and destination rectangles are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for dstrect.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopyF(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByRef srcrect As SDL_Rect, ByVal dstrect As IntPtr) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source and destination rectangles are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for both SDL_Rects.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopyF(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByVal srcrect As IntPtr, ByVal dstrect As IntPtr) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopyEx(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByRef srcrect As SDL_Rect, ByRef dstrect As SDL_FRect, ByVal angle As Double, ByRef center As SDL_FPoint, ByVal flip As SDL_RendererFlip) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source, destination, and/or center are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for srcrect.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopyEx(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByVal srcrect As IntPtr, ByRef dstrect As SDL_FRect, ByVal angle As Double, ByRef center As SDL_FPoint, ByVal flip As SDL_RendererFlip) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source, destination, and/or center are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for dstrect.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopyExF(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByRef srcrect As SDL_Rect, ByVal dstrect As IntPtr, ByVal angle As Double, ByRef center As SDL_FPoint, ByVal flip As SDL_RendererFlip) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source, destination, and/or center are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for center.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopyExF(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByRef srcrect As SDL_Rect, ByRef dstrect As SDL_FRect, ByVal angle As Double, ByVal center As IntPtr, ByVal flip As SDL_RendererFlip) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source, destination, and/or center are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for both
        ' 		 * srcrect and dstrect.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopyExF(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByVal srcrect As IntPtr, ByVal dstrect As IntPtr, ByVal angle As Double, ByRef center As SDL_FPoint, ByVal flip As SDL_RendererFlip) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source, destination, and/or center are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for both
        ' 		 * srcrect and center.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopyExF(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByVal srcrect As IntPtr, ByRef dstrect As SDL_FRect, ByVal angle As Double, ByVal center As IntPtr, ByVal flip As SDL_RendererFlip) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source, destination, and/or center are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for both
        ' 		 * dstrect and center.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopyExF(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByRef srcrect As SDL_Rect, ByVal dstrect As IntPtr, ByVal angle As Double, ByVal center As IntPtr, ByVal flip As SDL_RendererFlip) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source, destination, and/or center are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for all
        ' 		 * three parameters.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderCopyExF(ByVal renderer As IntPtr, ByVal texture As IntPtr, ByVal srcrect As IntPtr, ByVal dstrect As IntPtr, ByVal angle As Double, ByVal center As IntPtr, ByVal flip As SDL_RendererFlip) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderDrawPointF(ByVal renderer As IntPtr, ByVal x As Single, ByVal y As Single) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderDrawPointsF(ByVal renderer As IntPtr,
        <[In]> ByVal points As SDL_FPoint(), ByVal count As Integer) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderDrawLineF(ByVal renderer As IntPtr, ByVal x1 As Single, ByVal y1 As Single, ByVal x2 As Single, ByVal y2 As Single) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderDrawLinesF(ByVal renderer As IntPtr,
        <[In]> ByVal points As SDL_FPoint(), ByVal count As Integer) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderDrawRectF(ByVal renderer As IntPtr, ByRef rect As SDL_FRect) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, rect to an SDL_Rect*.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for rect.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderDrawRectF(ByVal renderer As IntPtr, ByVal rect As IntPtr) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderDrawRectsF(ByVal renderer As IntPtr,
        <[In]> ByVal rects As SDL_FRect(), ByVal count As Integer) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderFillRectF(ByVal renderer As IntPtr, ByRef rect As SDL_FRect) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, rect to an SDL_Rect*.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for rect.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderFillRectF(ByVal renderer As IntPtr, ByVal rect As IntPtr) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderFillRectsF(ByVal renderer As IntPtr,
        <[In]> ByVal rects As SDL_FRect(), ByVal count As Integer) As Integer
        End Function


#End Region

        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_RenderGetClipRect(ByVal renderer As IntPtr, <Out> ByRef rect As SDL_Rect)
        End Sub


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_RenderGetLogicalSize(ByVal renderer As IntPtr, <Out> ByRef w As Integer, <Out> ByRef h As Integer)
        End Sub


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_RenderGetScale(ByVal renderer As IntPtr, <Out> ByRef scaleX As Single, <Out> ByRef scaleY As Single)
        End Sub


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderGetViewport(ByVal renderer As IntPtr, <Out> ByRef rect As SDL_Rect) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_RenderPresent(ByVal renderer As IntPtr)
        End Sub


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderReadPixels(ByVal renderer As IntPtr, ByRef rect As SDL_Rect, ByVal format As UInteger, ByVal pixels As IntPtr, ByVal pitch As Integer) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderSetClipRect(ByVal renderer As IntPtr, ByRef rect As SDL_Rect) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for rect.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderSetClipRect(ByVal renderer As IntPtr, ByVal rect As IntPtr) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderSetLogicalSize(ByVal renderer As IntPtr, ByVal w As Integer, ByVal h As Integer) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderSetScale(ByVal renderer As IntPtr, ByVal scaleX As Single, ByVal scaleY As Single) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*
        ' 		 * Only available in 2.0.5 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderSetIntegerScale(ByVal renderer As IntPtr, ByVal enable As SDL_bool) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderSetViewport(ByVal renderer As IntPtr, ByRef rect As SDL_Rect) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetRenderDrawBlendMode(ByVal renderer As IntPtr, ByVal blendMode As SDL_BlendMode) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetRenderDrawColor(ByVal renderer As IntPtr, ByVal r As Byte, ByVal g As Byte, ByVal b As Byte, ByVal a As Byte) As Integer
        End Function


        ' renderer refers to an SDL_Renderer*, texture to an SDL_Texture* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetRenderTarget(ByVal renderer As IntPtr, ByVal texture As IntPtr) As Integer
        End Function


        ' texture refers to an SDL_Texture* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetTextureAlphaMod(ByVal texture As IntPtr, ByVal alpha As Byte) As Integer
        End Function


        ' texture refers to an SDL_Texture* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetTextureBlendMode(ByVal texture As IntPtr, ByVal blendMode As SDL_BlendMode) As Integer
        End Function


        ' texture refers to an SDL_Texture* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetTextureColorMod(ByVal texture As IntPtr, ByVal r As Byte, ByVal g As Byte, ByVal b As Byte) As Integer
        End Function


        ' texture refers to an SDL_Texture* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_UnlockTexture(ByVal texture As IntPtr)
        End Sub


        ' texture refers to an SDL_Texture* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_UpdateTexture(ByVal texture As IntPtr, ByRef rect As SDL_Rect, ByVal pixels As IntPtr, ByVal pitch As Integer) As Integer
        End Function


        ' texture refers to an SDL_Texture* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_UpdateTexture(ByVal texture As IntPtr, ByVal rect As IntPtr, ByVal pixels As IntPtr, ByVal pitch As Integer) As Integer
        End Function


        ' texture refers to an SDL_Texture*
        ' 		 * Only available in 2.0.1 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_UpdateYUVTexture(ByVal texture As IntPtr, ByRef rect As SDL_Rect, ByVal yPlane As IntPtr, ByVal yPitch As Integer, ByVal uPlane As IntPtr, ByVal uPitch As Integer, ByVal vPlane As IntPtr, ByVal vPitch As Integer) As Integer
        End Function


        ' renderer refers to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderTargetSupported(ByVal renderer As IntPtr) As SDL_bool
        End Function


        ' IntPtr refers to an SDL_Texture*, renderer to an SDL_Renderer* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetRenderTarget(ByVal renderer As IntPtr) As IntPtr
        End Function


        ' renderer refers to an SDL_Renderer*
        ' 		 * Only available in 2.0.8 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderGetMetalLayer(ByVal renderer As IntPtr) As IntPtr
        End Function


        ' renderer refers to an SDL_Renderer*
        ' 		 * Only available in 2.0.8 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderGetMetalCommandEncoder(ByVal renderer As IntPtr) As IntPtr
        End Function


        ' renderer refers to an SDL_Renderer*
        ' 		 * Only available in 2.0.4 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderIsClipEnabled(ByVal renderer As IntPtr) As SDL_bool
        End Function


        ' renderer refers to an SDL_Renderer*
        ' 		 * Only available in 2.0.10 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RenderFlush(ByVal renderer As IntPtr) As Integer
        End Function


#End Region

#Region "SDL_pixels.h"

        Public Function SDL_DEFINE_PIXELFOURCC(ByVal A As Byte, ByVal B As Byte, ByVal C As Byte, ByVal D As Byte) As UInteger
            Return SDL_FOURCC(A, B, C, D)
        End Function

        Public Function SDL_DEFINE_PIXELFORMAT(ByVal type As n_SDL_PixelType, ByVal order As UInteger, ByVal layout As SDL_PackedLayout, ByVal bits As Byte, ByVal bytes As Byte) As UInteger
            Return 1 << 28 Or CByte(type) << 24 Or CByte(order) << 20 Or CByte(layout) << 16 Or bits << 8 Or bytes
        End Function

        Public Function SDL_PIXELFLAG(ByVal X As UInteger) As Byte
            Return X >> 28 And &HF
        End Function

        Public Function SDL_PIXELTYPE(ByVal X As UInteger) As Byte
            Return X >> 24 And &HF
        End Function

        Public Function SDL_PIXELORDER(ByVal X As UInteger) As Byte
            Return X >> 20 And &HF
        End Function

        Public Function SDL_PIXELLAYOUT(ByVal X As UInteger) As Byte
            Return X >> 16 And &HF
        End Function

        Public Function SDL_BITSPERPIXEL(ByVal X As UInteger) As Byte
            Return X >> 8 And &HFF
        End Function

        Public Function SDL_BYTESPERPIXEL(ByVal X As UInteger) As Byte
            If SDL_ISPIXELFORMAT_FOURCC(X) Then

                If X = SDL_PIXELFORMAT_YUY2 OrElse X = SDL_PIXELFORMAT_UYVY OrElse X = SDL_PIXELFORMAT_YVYU Then
                    Return 2
                End If

                Return 1
            End If

            Return X And &HFF
        End Function

        Public Function SDL_ISPIXELFORMAT_INDEXED(ByVal format As UInteger) As Boolean
            If SDL_ISPIXELFORMAT_FOURCC(format) Then
                Return False
            End If

            Dim pType As n_SDL_PixelType = SDL_PIXELTYPE(format)
            Return pType = n_SDL_PixelType.SDL_PIXELTYPE_INDEX1 OrElse pType = n_SDL_PixelType.SDL_PIXELTYPE_INDEX4 OrElse pType = n_SDL_PixelType.SDL_PIXELTYPE_INDEX8
        End Function

        Public Function SDL_ISPIXELFORMAT_PACKED(ByVal format As UInteger) As Boolean
            If SDL_ISPIXELFORMAT_FOURCC(format) Then
                Return False
            End If

            Dim pType As n_SDL_PixelType = SDL_PIXELTYPE(format)
            Return pType = n_SDL_PixelType.SDL_PIXELTYPE_PACKED8 OrElse pType = n_SDL_PixelType.SDL_PIXELTYPE_PACKED16 OrElse pType = n_SDL_PixelType.SDL_PIXELTYPE_PACKED32
        End Function

        Public Function SDL_ISPIXELFORMAT_ARRAY(ByVal format As UInteger) As Boolean
            If SDL_ISPIXELFORMAT_FOURCC(format) Then
                Return False
            End If

            Dim pType As n_SDL_PixelType = SDL_PIXELTYPE(format)
            Return pType = n_SDL_PixelType.SDL_PIXELTYPE_ARRAYU8 OrElse pType = n_SDL_PixelType.SDL_PIXELTYPE_ARRAYU16 OrElse pType = n_SDL_PixelType.SDL_PIXELTYPE_ARRAYU32 OrElse pType = n_SDL_PixelType.SDL_PIXELTYPE_ARRAYF16 OrElse pType = n_SDL_PixelType.SDL_PIXELTYPE_ARRAYF32
        End Function

        Public Function SDL_ISPIXELFORMAT_ALPHA(ByVal format As UInteger) As Boolean
            If SDL_ISPIXELFORMAT_PACKED(format) Then
                Dim pOrder As SDL_PackedOrder = SDL_PIXELORDER(format)
                Return pOrder = SDL_PackedOrder.SDL_PACKEDORDER_ARGB OrElse pOrder = SDL_PackedOrder.SDL_PACKEDORDER_RGBA OrElse pOrder = SDL_PackedOrder.SDL_PACKEDORDER_ABGR OrElse pOrder = SDL_PackedOrder.SDL_PACKEDORDER_BGRA
            ElseIf SDL_ISPIXELFORMAT_ARRAY(format) Then
                Dim aOrder As SDL_ArrayOrder = SDL_PIXELORDER(format)
                Return aOrder = SDL_ArrayOrder.SDL_ARRAYORDER_ARGB OrElse aOrder = SDL_ArrayOrder.SDL_ARRAYORDER_RGBA OrElse aOrder = SDL_ArrayOrder.SDL_ARRAYORDER_ABGR OrElse aOrder = SDL_ArrayOrder.SDL_ARRAYORDER_BGRA
            End If

            Return False
        End Function

        Public Function SDL_ISPIXELFORMAT_FOURCC(ByVal format As UInteger) As Boolean
            Return format = 0 AndAlso SDL_PIXELFLAG(format) <> 1
        End Function

        Public Enum n_SDL_PixelType
            SDL_PIXELTYPE_UNKNOWN
            SDL_PIXELTYPE_INDEX1
            SDL_PIXELTYPE_INDEX4
            SDL_PIXELTYPE_INDEX8
            SDL_PIXELTYPE_PACKED8
            SDL_PIXELTYPE_PACKED16
            SDL_PIXELTYPE_PACKED32
            SDL_PIXELTYPE_ARRAYU8
            SDL_PIXELTYPE_ARRAYU16
            SDL_PIXELTYPE_ARRAYU32
            SDL_PIXELTYPE_ARRAYF16
            SDL_PIXELTYPE_ARRAYF32
        End Enum

        Public Enum SDL_BitmapOrder
            SDL_BITMAPORDER_NONE
            SDL_BITMAPORDER_4321
            SDL_BITMAPORDER_1234
        End Enum

        Public Enum SDL_PackedOrder
            SDL_PACKEDORDER_NONE
            SDL_PACKEDORDER_XRGB
            SDL_PACKEDORDER_RGBX
            SDL_PACKEDORDER_ARGB
            SDL_PACKEDORDER_RGBA
            SDL_PACKEDORDER_XBGR
            SDL_PACKEDORDER_BGRX
            SDL_PACKEDORDER_ABGR
            SDL_PACKEDORDER_BGRA
        End Enum

        Public Enum SDL_ArrayOrder
            SDL_ARRAYORDER_NONE
            SDL_ARRAYORDER_RGB
            SDL_ARRAYORDER_RGBA
            SDL_ARRAYORDER_ARGB
            SDL_ARRAYORDER_BGR
            SDL_ARRAYORDER_BGRA
            SDL_ARRAYORDER_ABGR
        End Enum

        Public Enum SDL_PackedLayout
            SDL_PACKEDLAYOUT_NONE
            SDL_PACKEDLAYOUT_332
            SDL_PACKEDLAYOUT_4444
            SDL_PACKEDLAYOUT_1555
            SDL_PACKEDLAYOUT_5551
            SDL_PACKEDLAYOUT_565
            SDL_PACKEDLAYOUT_8888
            SDL_PACKEDLAYOUT_2101010
            SDL_PACKEDLAYOUT_1010102
        End Enum

        Public ReadOnly SDL_PIXELFORMAT_UNKNOWN As UInteger = 0
        Public ReadOnly SDL_PIXELFORMAT_INDEX1LSB As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_INDEX1, SDL_BitmapOrder.SDL_BITMAPORDER_4321, 0, 1, 0)
        Public ReadOnly SDL_PIXELFORMAT_INDEX1MSB As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_INDEX1, SDL_BitmapOrder.SDL_BITMAPORDER_1234, 0, 1, 0)
        Public ReadOnly SDL_PIXELFORMAT_INDEX4LSB As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_INDEX4, SDL_BitmapOrder.SDL_BITMAPORDER_4321, 0, 4, 0)
        Public ReadOnly SDL_PIXELFORMAT_INDEX4MSB As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_INDEX4, SDL_BitmapOrder.SDL_BITMAPORDER_1234, 0, 4, 0)
        Public ReadOnly SDL_PIXELFORMAT_INDEX8 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_INDEX8, 0, 0, 8, 1)
        Public ReadOnly SDL_PIXELFORMAT_RGB332 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED8, SDL_PackedOrder.SDL_PACKEDORDER_XRGB, SDL_PackedLayout.SDL_PACKEDLAYOUT_332, 8, 1)
        Public ReadOnly SDL_PIXELFORMAT_RGB444 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED16, SDL_PackedOrder.SDL_PACKEDORDER_XRGB, SDL_PackedLayout.SDL_PACKEDLAYOUT_4444, 12, 2)
        Public ReadOnly SDL_PIXELFORMAT_BGR444 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED16, SDL_PackedOrder.SDL_PACKEDORDER_XBGR, SDL_PackedLayout.SDL_PACKEDLAYOUT_4444, 12, 2)
        Public ReadOnly SDL_PIXELFORMAT_RGB555 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED16, SDL_PackedOrder.SDL_PACKEDORDER_XRGB, SDL_PackedLayout.SDL_PACKEDLAYOUT_1555, 15, 2)
        Public ReadOnly SDL_PIXELFORMAT_BGR555 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_INDEX1, SDL_BitmapOrder.SDL_BITMAPORDER_4321, SDL_PackedLayout.SDL_PACKEDLAYOUT_1555, 15, 2)
        Public ReadOnly SDL_PIXELFORMAT_ARGB4444 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED16, SDL_PackedOrder.SDL_PACKEDORDER_ARGB, SDL_PackedLayout.SDL_PACKEDLAYOUT_4444, 16, 2)
        Public ReadOnly SDL_PIXELFORMAT_RGBA4444 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED16, SDL_PackedOrder.SDL_PACKEDORDER_RGBA, SDL_PackedLayout.SDL_PACKEDLAYOUT_4444, 16, 2)
        Public ReadOnly SDL_PIXELFORMAT_ABGR4444 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED16, SDL_PackedOrder.SDL_PACKEDORDER_ABGR, SDL_PackedLayout.SDL_PACKEDLAYOUT_4444, 16, 2)
        Public ReadOnly SDL_PIXELFORMAT_BGRA4444 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED16, SDL_PackedOrder.SDL_PACKEDORDER_BGRA, SDL_PackedLayout.SDL_PACKEDLAYOUT_4444, 16, 2)
        Public ReadOnly SDL_PIXELFORMAT_ARGB1555 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED16, SDL_PackedOrder.SDL_PACKEDORDER_ARGB, SDL_PackedLayout.SDL_PACKEDLAYOUT_1555, 16, 2)
        Public ReadOnly SDL_PIXELFORMAT_RGBA5551 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED16, SDL_PackedOrder.SDL_PACKEDORDER_RGBA, SDL_PackedLayout.SDL_PACKEDLAYOUT_5551, 16, 2)
        Public ReadOnly SDL_PIXELFORMAT_ABGR1555 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED16, SDL_PackedOrder.SDL_PACKEDORDER_ABGR, SDL_PackedLayout.SDL_PACKEDLAYOUT_1555, 16, 2)
        Public ReadOnly SDL_PIXELFORMAT_BGRA5551 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED16, SDL_PackedOrder.SDL_PACKEDORDER_BGRA, SDL_PackedLayout.SDL_PACKEDLAYOUT_5551, 16, 2)
        Public ReadOnly SDL_PIXELFORMAT_RGB565 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED16, SDL_PackedOrder.SDL_PACKEDORDER_XRGB, SDL_PackedLayout.SDL_PACKEDLAYOUT_565, 16, 2)
        Public ReadOnly SDL_PIXELFORMAT_BGR565 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED16, SDL_PackedOrder.SDL_PACKEDORDER_XBGR, SDL_PackedLayout.SDL_PACKEDLAYOUT_565, 16, 2)
        Public ReadOnly SDL_PIXELFORMAT_RGB24 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_ARRAYU8, SDL_ArrayOrder.SDL_ARRAYORDER_RGB, 0, 24, 3)
        Public ReadOnly SDL_PIXELFORMAT_BGR24 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_ARRAYU8, SDL_ArrayOrder.SDL_ARRAYORDER_BGR, 0, 24, 3)
        Public ReadOnly SDL_PIXELFORMAT_RGB888 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED32, SDL_PackedOrder.SDL_PACKEDORDER_XRGB, SDL_PackedLayout.SDL_PACKEDLAYOUT_8888, 24, 4)
        Public ReadOnly SDL_PIXELFORMAT_RGBX8888 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED32, SDL_PackedOrder.SDL_PACKEDORDER_RGBX, SDL_PackedLayout.SDL_PACKEDLAYOUT_8888, 24, 4)
        Public ReadOnly SDL_PIXELFORMAT_BGR888 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED32, SDL_PackedOrder.SDL_PACKEDORDER_XBGR, SDL_PackedLayout.SDL_PACKEDLAYOUT_8888, 24, 4)
        Public ReadOnly SDL_PIXELFORMAT_BGRX8888 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED32, SDL_PackedOrder.SDL_PACKEDORDER_BGRX, SDL_PackedLayout.SDL_PACKEDLAYOUT_8888, 24, 4)
        Public ReadOnly SDL_PIXELFORMAT_ARGB8888 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED32, SDL_PackedOrder.SDL_PACKEDORDER_ARGB, SDL_PackedLayout.SDL_PACKEDLAYOUT_8888, 32, 4)
        Public ReadOnly SDL_PIXELFORMAT_RGBA8888 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED32, SDL_PackedOrder.SDL_PACKEDORDER_RGBA, SDL_PackedLayout.SDL_PACKEDLAYOUT_8888, 32, 4)
        Public ReadOnly SDL_PIXELFORMAT_ABGR8888 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED32, SDL_PackedOrder.SDL_PACKEDORDER_ABGR, SDL_PackedLayout.SDL_PACKEDLAYOUT_8888, 32, 4)
        Public ReadOnly SDL_PIXELFORMAT_BGRA8888 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED32, SDL_PackedOrder.SDL_PACKEDORDER_BGRA, SDL_PackedLayout.SDL_PACKEDLAYOUT_8888, 32, 4)
        Public ReadOnly SDL_PIXELFORMAT_ARGB2101010 As UInteger = SDL_DEFINE_PIXELFORMAT(n_SDL_PixelType.SDL_PIXELTYPE_PACKED32, SDL_PackedOrder.SDL_PACKEDORDER_ARGB, SDL_PackedLayout.SDL_PACKEDLAYOUT_2101010, 32, 4)
        Public ReadOnly SDL_PIXELFORMAT_YV12 As UInteger = SDL_DEFINE_PIXELFOURCC(Microsoft.VisualBasic.AscW("Y"c), Microsoft.VisualBasic.AscW("V"c), Microsoft.VisualBasic.AscW("1"c), Microsoft.VisualBasic.AscW("2"c))
        Public ReadOnly SDL_PIXELFORMAT_IYUV As UInteger = SDL_DEFINE_PIXELFOURCC(Microsoft.VisualBasic.AscW("I"c), Microsoft.VisualBasic.AscW("Y"c), Microsoft.VisualBasic.AscW("U"c), Microsoft.VisualBasic.AscW("V"c))
        Public ReadOnly SDL_PIXELFORMAT_YUY2 As UInteger = SDL_DEFINE_PIXELFOURCC(Microsoft.VisualBasic.AscW("Y"c), Microsoft.VisualBasic.AscW("U"c), Microsoft.VisualBasic.AscW("Y"c), Microsoft.VisualBasic.AscW("2"c))
        Public ReadOnly SDL_PIXELFORMAT_UYVY As UInteger = SDL_DEFINE_PIXELFOURCC(Microsoft.VisualBasic.AscW("U"c), Microsoft.VisualBasic.AscW("Y"c), Microsoft.VisualBasic.AscW("V"c), Microsoft.VisualBasic.AscW("Y"c))
        Public ReadOnly SDL_PIXELFORMAT_YVYU As UInteger = SDL_DEFINE_PIXELFOURCC(Microsoft.VisualBasic.AscW("Y"c), Microsoft.VisualBasic.AscW("V"c), Microsoft.VisualBasic.AscW("Y"c), Microsoft.VisualBasic.AscW("U"c))

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_Color
            Public r As Byte
            Public g As Byte
            Public b As Byte
            Public a As Byte
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_Palette
            Public ncolors As Integer
            Public colors As IntPtr
            Public version As Integer
            Public refcount As Integer
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_PixelFormat
            Public format As UInteger
            Public palette As IntPtr ' SDL_Palette*
            Public BitsPerPixel As Byte
            Public BytesPerPixel As Byte
            Public Rmask As UInteger
            Public Gmask As UInteger
            Public Bmask As UInteger
            Public Amask As UInteger
            Public Rloss As Byte
            Public Gloss As Byte
            Public Bloss As Byte
            Public Aloss As Byte
            Public Rshift As Byte
            Public Gshift As Byte
            Public Bshift As Byte
            Public Ashift As Byte
            Public refcount As Integer
            Public [next] As IntPtr ' SDL_PixelFormat*
        End Structure


        ' IntPtr refers to an SDL_PixelFormat* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_AllocFormat(ByVal pixel_format As UInteger) As IntPtr
        End Function


        ' IntPtr refers to an SDL_Palette* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_AllocPalette(ByVal ncolors As Integer) As IntPtr
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_CalculateGammaRamp(ByVal gamma As Single,
        <Out()>
        <MarshalAs(UnmanagedType.LPArray, ArraySubType:=UnmanagedType.U2, SizeConst:=256)> ByVal ramp As UShort())
        End Sub


        ' format refers to an SDL_PixelFormat* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_FreeFormat(ByVal format As IntPtr)
        End Sub


        ' palette refers to an SDL_Palette* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_FreePalette(ByVal palette As IntPtr)
        End Sub

        <DllImport(nativeLibName, EntryPoint:="SDL_GetPixelFormatName", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GetPixelFormatName(ByVal format As UInteger) As IntPtr
        End Function

        Public Function SDL_GetPixelFormatName(ByVal format As UInteger) As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GetPixelFormatName(format))
        End Function


        ' format refers to an SDL_PixelFormat* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_GetRGB(ByVal pixel As UInteger, ByVal format As IntPtr, <Out> ByRef r As Byte, <Out> ByRef g As Byte, <Out> ByRef b As Byte)
        End Sub


        ' format refers to an SDL_PixelFormat* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_GetRGBA(ByVal pixel As UInteger, ByVal format As IntPtr, <Out> ByRef r As Byte, <Out> ByRef g As Byte, <Out> ByRef b As Byte, <Out> ByRef a As Byte)
        End Sub


        ' format refers to an SDL_PixelFormat* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_MapRGB(ByVal format As IntPtr, ByVal r As Byte, ByVal g As Byte, ByVal b As Byte) As UInteger
        End Function


        ' format refers to an SDL_PixelFormat* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_MapRGBA(ByVal format As IntPtr, ByVal r As Byte, ByVal g As Byte, ByVal b As Byte, ByVal a As Byte) As UInteger
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_MasksToPixelFormatEnum(ByVal bpp As Integer, ByVal Rmask As UInteger, ByVal Gmask As UInteger, ByVal Bmask As UInteger, ByVal Amask As UInteger) As UInteger
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_PixelFormatEnumToMasks(ByVal format As UInteger, <Out> ByRef bpp As Integer, <Out> ByRef Rmask As UInteger, <Out> ByRef Gmask As UInteger, <Out> ByRef Bmask As UInteger, <Out> ByRef Amask As UInteger) As SDL_bool
        End Function


        ' palette refers to an SDL_Palette* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetPaletteColors(ByVal palette As IntPtr,
        <[In]> ByVal colors As SDL_Color(), ByVal firstcolor As Integer, ByVal ncolors As Integer) As Integer
        End Function


        ' format and palette refer to an SDL_PixelFormat* and SDL_Palette* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetPixelFormatPalette(ByVal format As IntPtr, ByVal palette As IntPtr) As Integer
        End Function


#End Region

#Region "SDL_rect.h"

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_Point
            Public x As Integer
            Public y As Integer
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_Rect
            Public x As Integer
            Public y As Integer
            Public w As Integer
            Public h As Integer
        End Structure


        ' Only available in 2.0.10 or higher. 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_FPoint
            Public x As Single
            Public y As Single
        End Structure


        ' Only available in 2.0.10 or higher. 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_FRect
            Public x As Single
            Public y As Single
            Public w As Single
            Public h As Single
        End Structure


        ' Only available in 2.0.4 or higher. 
        Public Function SDL_PointInRect(ByRef p As SDL_Point, ByRef r As SDL_Rect) As SDL_bool
            Return If(p.x >= r.x AndAlso p.x < r.x + r.w AndAlso p.y >= r.y AndAlso p.y < r.y + r.h, SDL_bool.SDL_TRUE, SDL_bool.SDL_FALSE)
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_EnclosePoints(
        <[In]> ByVal points As SDL_Point(), ByVal count As Integer, ByRef clip As SDL_Rect, <Out> ByRef result As SDL_Rect) As SDL_bool
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HasIntersection(ByRef A As SDL_Rect, ByRef B As SDL_Rect) As SDL_bool
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_IntersectRect(ByRef A As SDL_Rect, ByRef B As SDL_Rect, <Out> ByRef result As SDL_Rect) As SDL_bool
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_IntersectRectAndLine(ByRef rect As SDL_Rect, ByRef X1 As Integer, ByRef Y1 As Integer, ByRef X2 As Integer, ByRef Y2 As Integer) As SDL_bool
        End Function

        Public Function SDL_RectEmpty(ByRef r As SDL_Rect) As SDL_bool
            Return If(r.w <= 0 OrElse r.h <= 0, SDL_bool.SDL_TRUE, SDL_bool.SDL_FALSE)
        End Function

        Public Function SDL_RectEquals(ByRef a As SDL_Rect, ByRef b As SDL_Rect) As SDL_bool
            Return If(a.x = b.x AndAlso a.y = b.y AndAlso a.w = b.w AndAlso a.h = b.h, SDL_bool.SDL_TRUE, SDL_bool.SDL_FALSE)
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_UnionRect(ByRef A As SDL_Rect, ByRef B As SDL_Rect, <Out> ByRef result As SDL_Rect)
        End Sub


#End Region

#Region "SDL_surface.h"

        Public Const SDL_SWSURFACE As UInteger = &H0
        Public Const SDL_PREALLOC As UInteger = &H1
        Public Const SDL_RLEACCEL As UInteger = &H2
        Public Const SDL_DONTFREE As UInteger = &H4

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_Surface
            Public flags As UInteger
            Public format As IntPtr ' SDL_PixelFormat*
            Public w As Integer
            Public h As Integer
            Public pitch As Integer
            Public pixels As IntPtr ' void*
            Public userdata As IntPtr ' void*
            Public locked As Integer
            Public lock_data As IntPtr ' void*
            Public clip_rect As SDL_Rect
            Public map As IntPtr ' SDL_BlitMap*
            Public refcount As Integer
        End Structure


        ' surface refers to an SDL_Surface* 
        Public Function SDL_MUSTLOCK(ByVal surface As IntPtr) As Boolean
            Dim sur As SDL_Surface
            sur = CType(Marshal.PtrToStructure(surface, GetType(SDL_Surface)), SDL_Surface)
            Return (sur.flags And SDL_RLEACCEL) <> 0
        End Function


        ' src and dst refer to an SDL_Surface* 
        <DllImport(nativeLibName, EntryPoint:="SDL_UpperBlit", CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_BlitSurface(ByVal src As IntPtr, ByRef srcrect As SDL_Rect, ByVal dst As IntPtr, ByRef dstrect As SDL_Rect) As Integer
        End Function


        ' src and dst refer to an SDL_Surface*
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source and destination rectangles are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for srcrect.
        ' 		 
        <DllImport(nativeLibName, EntryPoint:="SDL_UpperBlit", CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_BlitSurface(ByVal src As IntPtr, ByVal srcrect As IntPtr, ByVal dst As IntPtr, ByRef dstrect As SDL_Rect) As Integer
        End Function


        ' src and dst refer to an SDL_Surface*
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source and destination rectangles are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for dstrect.
        ' 		 
        <DllImport(nativeLibName, EntryPoint:="SDL_UpperBlit", CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_BlitSurface(ByVal src As IntPtr, ByRef srcrect As SDL_Rect, ByVal dst As IntPtr, ByVal dstrect As IntPtr) As Integer
        End Function


        ' src and dst refer to an SDL_Surface*
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source and destination rectangles are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for both SDL_Rects.
        ' 		 
        <DllImport(nativeLibName, EntryPoint:="SDL_UpperBlit", CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_BlitSurface(ByVal src As IntPtr, ByVal srcrect As IntPtr, ByVal dst As IntPtr, ByVal dstrect As IntPtr) As Integer
        End Function


        ' src and dst refer to an SDL_Surface* 
        <DllImport(nativeLibName, EntryPoint:="SDL_UpperBlitScaled", CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_BlitScaled(ByVal src As IntPtr, ByRef srcrect As SDL_Rect, ByVal dst As IntPtr, ByRef dstrect As SDL_Rect) As Integer
        End Function


        ' src and dst refer to an SDL_Surface*
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source and destination rectangles are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for srcrect.
        ' 		 
        <DllImport(nativeLibName, EntryPoint:="SDL_UpperBlitScaled", CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_BlitScaled(ByVal src As IntPtr, ByVal srcrect As IntPtr, ByVal dst As IntPtr, ByRef dstrect As SDL_Rect) As Integer
        End Function


        ' src and dst refer to an SDL_Surface*
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source and destination rectangles are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for dstrect.
        ' 		 
        <DllImport(nativeLibName, EntryPoint:="SDL_UpperBlitScaled", CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_BlitScaled(ByVal src As IntPtr, ByRef srcrect As SDL_Rect, ByVal dst As IntPtr, ByVal dstrect As IntPtr) As Integer
        End Function


        ' src and dst refer to an SDL_Surface*
        ' 		 * Internally, this function contains logic to use default values when
        ' 		 * source and destination rectangles are passed as NULL.
        ' 		 * This overload allows for IntPtr.Zero (null) to be passed for both SDL_Rects.
        ' 		 
        <DllImport(nativeLibName, EntryPoint:="SDL_UpperBlitScaled", CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_BlitScaled(ByVal src As IntPtr, ByVal srcrect As IntPtr, ByVal dst As IntPtr, ByVal dstrect As IntPtr) As Integer
        End Function


        ' src and dst are void* pointers 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_ConvertPixels(ByVal width As Integer, ByVal height As Integer, ByVal src_format As UInteger, ByVal src As IntPtr, ByVal src_pitch As Integer, ByVal dst_format As UInteger, ByVal dst As IntPtr, ByVal dst_pitch As Integer) As Integer
        End Function


        ' IntPtr refers to an SDL_Surface*
        ' 		 * src refers to an SDL_Surface*
        ' 		 * fmt refers to an SDL_PixelFormat*
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_ConvertSurface(ByVal src As IntPtr, ByVal fmt As IntPtr, ByVal flags As UInteger) As IntPtr
        End Function


        ' IntPtr refers to an SDL_Surface*, src to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_ConvertSurfaceFormat(ByVal src As IntPtr, ByVal pixel_format As UInteger, ByVal flags As UInteger) As IntPtr
        End Function


        ' IntPtr refers to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_CreateRGBSurface(ByVal flags As UInteger, ByVal width As Integer, ByVal height As Integer, ByVal depth As Integer, ByVal Rmask As UInteger, ByVal Gmask As UInteger, ByVal Bmask As UInteger, ByVal Amask As UInteger) As IntPtr
        End Function


        ' IntPtr refers to an SDL_Surface*, pixels to a void* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_CreateRGBSurfaceFrom(ByVal pixels As IntPtr, ByVal width As Integer, ByVal height As Integer, ByVal depth As Integer, ByVal pitch As Integer, ByVal Rmask As UInteger, ByVal Gmask As UInteger, ByVal Bmask As UInteger, ByVal Amask As UInteger) As IntPtr
        End Function


        ' IntPtr refers to an SDL_Surface*
        ' 		 * Only available in 2.0.5 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_CreateRGBSurfaceWithFormat(ByVal flags As UInteger, ByVal width As Integer, ByVal height As Integer, ByVal depth As Integer, ByVal format As UInteger) As IntPtr
        End Function


        ' IntPtr refers to an SDL_Surface*, pixels to a void*
        ' 		 * Only available in 2.0.5 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_CreateRGBSurfaceWithFormatFrom(ByVal pixels As IntPtr, ByVal width As Integer, ByVal height As Integer, ByVal depth As Integer, ByVal pitch As Integer, ByVal format As UInteger) As IntPtr
        End Function


        ' dst refers to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_FillRect(ByVal dst As IntPtr, ByRef rect As SDL_Rect, ByVal color As UInteger) As Integer
        End Function


        ' dst refers to an SDL_Surface*.
        ' 		 * This overload allows passing NULL to rect.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_FillRect(ByVal dst As IntPtr, ByVal rect As IntPtr, ByVal color As UInteger) As Integer
        End Function


        ' dst refers to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_FillRects(ByVal dst As IntPtr,
        <[In]> ByVal rects As SDL_Rect(), ByVal count As Integer, ByVal color As UInteger) As Integer
        End Function


        ' surface refers to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_FreeSurface(ByVal surface As IntPtr)
        End Sub


        ' surface refers to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_GetClipRect(ByVal surface As IntPtr, <Out> ByRef rect As SDL_Rect)
        End Sub


        ' surface refers to an SDL_Surface*.
        ' 		 * Only available in 2.0.9 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HasColorKey(ByVal surface As IntPtr) As SDL_bool
        End Function


        ' surface refers to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetColorKey(ByVal surface As IntPtr, <Out> ByRef key As UInteger) As Integer
        End Function


        ' surface refers to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetSurfaceAlphaMod(ByVal surface As IntPtr, <Out> ByRef alpha As Byte) As Integer
        End Function


        ' surface refers to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetSurfaceBlendMode(ByVal surface As IntPtr, <Out> ByRef blendMode As SDL_BlendMode) As Integer
        End Function


        ' surface refers to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetSurfaceColorMod(ByVal surface As IntPtr, <Out> ByRef r As Byte, <Out> ByRef g As Byte, <Out> ByRef b As Byte) As Integer
        End Function


        ' These are for SDL_LoadBMP, which is a macro in the SDL headers. 
        ' IntPtr refers to an SDL_Surface* 
        ' THIS IS AN RWops FUNCTION! 
        <DllImport(nativeLibName, EntryPoint:="SDL_LoadBMP_RW", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_LoadBMP_RW(ByVal src As IntPtr, ByVal freesrc As Integer) As IntPtr
        End Function

        Public Function SDL_LoadBMP(ByVal file As String) As IntPtr
            Dim rwops As IntPtr = SDL.SDL_RWFromFile(file, "rb")
            Return INTERNAL_SDL_LoadBMP_RW(rwops, 1)
        End Function


        ' surface refers to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_LockSurface(ByVal surface As IntPtr) As Integer
        End Function


        ' src and dst refer to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_LowerBlit(ByVal src As IntPtr, ByRef srcrect As SDL_Rect, ByVal dst As IntPtr, ByRef dstrect As SDL_Rect) As Integer
        End Function


        ' src and dst refer to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_LowerBlitScaled(ByVal src As IntPtr, ByRef srcrect As SDL_Rect, ByVal dst As IntPtr, ByRef dstrect As SDL_Rect) As Integer
        End Function


        ' These are for SDL_SaveBMP, which is a macro in the SDL headers. 
        ' IntPtr refers to an SDL_Surface* 
        ' THIS IS AN RWops FUNCTION! 
        <DllImport(nativeLibName, EntryPoint:="SDL_SaveBMP_RW", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_SaveBMP_RW(ByVal surface As IntPtr, ByVal src As IntPtr, ByVal freesrc As Integer) As Integer
        End Function

        Public Function SDL_SaveBMP(ByVal surface As IntPtr, ByVal file As String) As Integer
            Dim rwops As IntPtr = SDL.SDL_RWFromFile(file, "wb")
            Return INTERNAL_SDL_SaveBMP_RW(surface, rwops, 1)
        End Function


        ' surface refers to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetClipRect(ByVal surface As IntPtr, ByRef rect As SDL_Rect) As SDL_bool
        End Function


        ' surface refers to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetColorKey(ByVal surface As IntPtr, ByVal flag As Integer, ByVal key As UInteger) As Integer
        End Function


        ' surface refers to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetSurfaceAlphaMod(ByVal surface As IntPtr, ByVal alpha As Byte) As Integer
        End Function


        ' surface refers to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetSurfaceBlendMode(ByVal surface As IntPtr, ByVal blendMode As SDL_BlendMode) As Integer
        End Function


        ' surface refers to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetSurfaceColorMod(ByVal surface As IntPtr, ByVal r As Byte, ByVal g As Byte, ByVal b As Byte) As Integer
        End Function


        ' surface refers to an SDL_Surface*, palette to an SDL_Palette* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetSurfacePalette(ByVal surface As IntPtr, ByVal palette As IntPtr) As Integer
        End Function


        ' surface refers to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetSurfaceRLE(ByVal surface As IntPtr, ByVal flag As Integer) As Integer
        End Function


        ' src and dst refer to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SoftStretch(ByVal src As IntPtr, ByRef srcrect As SDL_Rect, ByVal dst As IntPtr, ByRef dstrect As SDL_Rect) As Integer
        End Function


        ' surface refers to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_UnlockSurface(ByVal surface As IntPtr)
        End Sub


        ' src and dst refer to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_UpperBlit(ByVal src As IntPtr, ByRef srcrect As SDL_Rect, ByVal dst As IntPtr, ByRef dstrect As SDL_Rect) As Integer
        End Function


        ' src and dst refer to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_UpperBlitScaled(ByVal src As IntPtr, ByRef srcrect As SDL_Rect, ByVal dst As IntPtr, ByRef dstrect As SDL_Rect) As Integer
        End Function


        ' surface and IntPtr refer to an SDL_Surface* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_DuplicateSurface(ByVal surface As IntPtr) As IntPtr
        End Function


#End Region

#Region "SDL_clipboard.h"

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HasClipboardText() As SDL_bool
        End Function

        <DllImport(nativeLibName, EntryPoint:="SDL_GetClipboardText", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GetClipboardText() As IntPtr
        End Function

        Public Function SDL_GetClipboardText() As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GetClipboardText(), True)
        End Function

        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_SetClipboardText", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe int INTERNAL_SDL_SetClipboardText(
        ''' 			byte* text
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe int SDL_SetClipboardText(
        ''' 			string text
        ''' 		)
        ''' 		{
        ''' 			byte* utf8Text = SDL2.SDL.Utf8Encode(text);
        ''' 			int result = SDL2.SDL.INTERNAL_SDL_SetClipboardText(
        ''' 				utf8Text
        ''' 			);
        ''' 			System.Runtime.InteropServices.Marshal.FreeHGlobal((System.IntPtr)utf8Text);
        ''' 			return result;
        ''' 		}
        ''' 
        ''' 

#End Region

#Region "SDL_events.h"

        ' General keyboard/mouse state definitions. 
        Public Const SDL_PRESSED As Byte = 1
        Public Const SDL_RELEASED As Byte = 0

        ' Default size is according to SDL2 default. 
        Public Const SDL_TEXTEDITINGEVENT_TEXT_SIZE As Integer = 32
        Public Const SDL_TEXTINPUTEVENT_TEXT_SIZE As Integer = 32


        ' The types of events that can be delivered. 
        Public Enum SDL_EventType As UInteger
            SDL_FIRSTEVENT = 0

            ' Application events 
            SDL_QUIT = &H100

            ' iOS/Android/WinRT app events 
            SDL_APP_TERMINATING
            SDL_APP_LOWMEMORY
            SDL_APP_WILLENTERBACKGROUND
            SDL_APP_DIDENTERBACKGROUND
            SDL_APP_WILLENTERFOREGROUND
            SDL_APP_DIDENTERFOREGROUND

            ' Display events 
            ' Only available in SDL 2.0.9 or higher. 
            SDL_DISPLAYEVENT = &H150

            ' Window events 
            SDL_WINDOWEVENT = &H200
            SDL_SYSWMEVENT

            ' Keyboard events 
            SDL_KEYDOWN = &H300
            SDL_KEYUP
            SDL_TEXTEDITING
            SDL_TEXTINPUT
            SDL_KEYMAPCHANGED

            ' Mouse events 
            SDL_MOUSEMOTION = &H400
            SDL_MOUSEBUTTONDOWN
            SDL_MOUSEBUTTONUP
            SDL_MOUSEWHEEL

            ' Joystick events 
            SDL_JOYAXISMOTION = &H600
            SDL_JOYBALLMOTION
            SDL_JOYHATMOTION
            SDL_JOYBUTTONDOWN
            SDL_JOYBUTTONUP
            SDL_JOYDEVICEADDED
            SDL_JOYDEVICEREMOVED

            ' Game controller events 
            SDL_CONTROLLERAXISMOTION = &H650
            SDL_CONTROLLERBUTTONDOWN
            SDL_CONTROLLERBUTTONUP
            SDL_CONTROLLERDEVICEADDED
            SDL_CONTROLLERDEVICEREMOVED
            SDL_CONTROLLERDEVICEREMAPPED

            ' Touch events 
            SDL_FINGERDOWN = &H700
            SDL_FINGERUP
            SDL_FINGERMOTION

            ' Gesture events 
            SDL_DOLLARGESTURE = &H800
            SDL_DOLLARRECORD
            SDL_MULTIGESTURE

            ' Clipboard events 
            SDL_CLIPBOARDUPDATE = &H900

            ' Drag and drop events 
            SDL_DROPFILE = &H1000
            ' Only available in 2.0.4 or higher. 
            SDL_DROPTEXT
            SDL_DROPBEGIN
            SDL_DROPCOMPLETE

            ' Audio hotplug events 
            ' Only available in SDL 2.0.4 or higher. 
            SDL_AUDIODEVICEADDED = &H1100
            SDL_AUDIODEVICEREMOVED

            ' Sensor events 
            ' Only available in SDL 2.0.9 or higher. 
            SDL_SENSORUPDATE = &H1200

            ' Render events 
            ' Only available in SDL 2.0.2 or higher. 
            SDL_RENDER_TARGETS_RESET = &H2000
            ' Only available in SDL 2.0.4 or higher. 
            SDL_RENDER_DEVICE_RESET

            ' Events SDL_USEREVENT through SDL_LASTEVENT are for
            ' 			 * your use, and should be allocated with
            ' 			 * SDL_RegisterEvents()
            ' 			 
            SDL_USEREVENT = &H8000

            ' The last event, used for bouding arrays. 
            SDL_LASTEVENT = &HFFFF
        End Enum


        ' Only available in 2.0.4 or higher. 
        Public Enum SDL_MouseWheelDirection As UInteger
            SDL_MOUSEWHEEL_NORMAL
            SDL_MOUSEWHEEL_FLIPPED
        End Enum


        ' Fields shared by every event 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_GenericEvent
            Public type As SDL_EventType
            Public timestamp As UInteger
        End Structure


        ' Ignore private members used for padding in this struct
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_DisplayEvent
            Public type As SDL_EventType
            Public timestamp As UInteger
            Public display As UInteger
            Public displayEvent As SDL_DisplayEventID ' event, lolC#
            Private padding1 As Byte
            Private padding2 As Byte
            Private padding3 As Byte
            Public data1 As Integer
        End Structure


        ' Ignore private members used for padding in this struct
        ' Window state change event data (event.window.*) 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_WindowEvent
            Public type As SDL_EventType
            Public timestamp As UInteger
            Public windowID As UInteger
            Public windowEvent As SDL_WindowEventID ' event, lolC#
            Private padding1 As Byte
            Private padding2 As Byte
            Private padding3 As Byte
            Public data1 As Integer
            Public data2 As Integer
        End Structure


        ' Ignore private members used for padding in this struct
        ' Keyboard button event structure (event.key.*) 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_KeyboardEvent
            Public type As SDL_EventType
            Public timestamp As UInteger
            Public windowID As UInteger
            Public state As Byte
            Public repeat As Byte ' non-zero if this is a repeat 
            Private padding2 As Byte
            Private padding3 As Byte
            Public keysym As SDL_Keysym

        End Structure

        ''' Cannot convert StructDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitStructDeclaration(StructDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' #pragma warning restore 0169
        ''' 
        ''' 		[System.Runtime.InteropServices.@StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        ''' 		public unsafe struct SDL_TextEditingEvent
        ''' 		{
        ''' 			public SDL2.SDL.SDL_EventType type;
        ''' 			public System.UInt32 timestamp;
        ''' 			public System.UInt32 windowID;
        ''' 			public fixed byte text[SDL_TEXTEDITINGEVENT_TEXT_SIZE];
        ''' 			public System.Int32 start;
        ''' 			public System.Int32 length;
        ''' 		}
        ''' 		
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_TextEditingEvent
            Public type As SDL.SDL_EventType
            Public timestamp As UInt32
            Public windowID As UInt32
            <VBFixedArray(SDL_TEXTEDITINGEVENT_TEXT_SIZE)> Public text() As Byte
            Public start As Int32
            Public length As Int32
        End Structure

        ''' 
        ''' 
        ''' Cannot convert StructDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitStructDeclaration(StructDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		[System.Runtime.InteropServices.@StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        ''' 		public unsafe struct SDL_TextInputEvent
        ''' 		{
        ''' 			public SDL2.SDL.SDL_EventType type;
        ''' 			public System.UInt32 timestamp;
        ''' 			public System.UInt32 windowID;
        ''' 			public fixed byte text[SDL_TEXTINPUTEVENT_TEXT_SIZE];
        ''' 		}
        ''' 
        ''' 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_TextInputEvent
            Public type As SDL.SDL_EventType
            Public timestamp As UInt32
            Public windowID As UInt32
            <VBFixedArray(SDL_TEXTINPUTEVENT_TEXT_SIZE)> Public text() As Byte
        End Structure

        ' Ignore private members used for padding in this struct
        ' Mouse motion event structure (event.motion.*) 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_MouseMotionEvent
            Public type As SDL_EventType
            Public timestamp As UInteger
            Public windowID As UInteger
            Public which As UInteger
            Public state As Byte ' bitmask of buttons 
            Private padding1 As Byte
            Private padding2 As Byte
            Private padding3 As Byte
            Public x As Integer
            Public y As Integer
            Public xrel As Integer
            Public yrel As Integer
        End Structure


        ' Ignore private members used for padding in this struct
        ' Mouse button event structure (event.button.*) 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_MouseButtonEvent
            Public type As SDL_EventType
            Public timestamp As UInteger
            Public windowID As UInteger
            Public which As UInteger
            Public button As Byte ' button id 
            Public state As Byte ' SDL_PRESSED or SDL_RELEASED 
            Public clicks As Byte ' 1 for single-click, 2 for double-click, etc. 
            Private padding1 As Byte
            Public x As Integer
            Public y As Integer
        End Structure


        ' Mouse wheel event structure (event.wheel.*) 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_MouseWheelEvent
            Public type As SDL_EventType
            Public timestamp As UInteger
            Public windowID As UInteger
            Public which As UInteger
            Public x As Integer ' amount scrolled horizontally 
            Public y As Integer ' amount scrolled vertically 
            Public direction As UInteger ' Set to one of the SDL_MOUSEWHEEL_* defines 
        End Structure


        ' Ignore private members used for padding in this struct
        ' Joystick axis motion event structure (event.jaxis.*) 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_JoyAxisEvent
            Public type As SDL_EventType
            Public timestamp As UInteger
            Public which As Integer ' SDL_JoystickID 
            Public axis As Byte
            Private padding1 As Byte
            Private padding2 As Byte
            Private padding3 As Byte
            Public axisValue As Short ' value, lolC# 
            Public padding4 As UShort
        End Structure


        ' Ignore private members used for padding in this struct
        ' Joystick trackball motion event structure (event.jball.*) 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_JoyBallEvent
            Public type As SDL_EventType
            Public timestamp As UInteger
            Public which As Integer ' SDL_JoystickID 
            Public ball As Byte
            Private padding1 As Byte
            Private padding2 As Byte
            Private padding3 As Byte
            Public xrel As Short
            Public yrel As Short
        End Structure


        ' Ignore private members used for padding in this struct
        ' Joystick hat position change event struct (event.jhat.*) 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_JoyHatEvent
            Public type As SDL_EventType
            Public timestamp As UInteger
            Public which As Integer ' SDL_JoystickID 
            Public hat As Byte ' index of the hat 
            Public hatValue As Byte ' value, lolC# 
            Private padding1 As Byte
            Private padding2 As Byte
        End Structure


        ' Ignore private members used for padding in this struct
        ' Joystick button event structure (event.jbutton.*) 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_JoyButtonEvent
            Public type As SDL_EventType
            Public timestamp As UInteger
            Public which As Integer ' SDL_JoystickID 
            Public button As Byte
            Public state As Byte ' SDL_PRESSED or SDL_RELEASED 
            Private padding1 As Byte
            Private padding2 As Byte
        End Structure


        ' Joystick device event structure (event.jdevice.*) 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_JoyDeviceEvent
            Public type As SDL_EventType
            Public timestamp As UInteger
            Public which As Integer ' SDL_JoystickID 
        End Structure


        ' Ignore private members used for padding in this struct
        ' Game controller axis motion event (event.caxis.*) 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_ControllerAxisEvent
            Public type As SDL_EventType
            Public timestamp As UInteger
            Public which As Integer ' SDL_JoystickID 
            Public axis As Byte
            Private padding1 As Byte
            Private padding2 As Byte
            Private padding3 As Byte
            Public axisValue As Short ' value, lolC# 
            Private padding4 As UShort
        End Structure


        ' Ignore private members used for padding in this struct
        ' Game controller button event (event.cbutton.*) 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_ControllerButtonEvent
            Public type As SDL_EventType
            Public timestamp As UInteger
            Public which As Integer ' SDL_JoystickID 
            Public button As Byte
            Public state As Byte
            Private padding1 As Byte
            Private padding2 As Byte
        End Structure


        ' Game controller device event (event.cdevice.*) 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_ControllerDeviceEvent
            Public type As SDL_EventType
            Public timestamp As UInteger
            Public which As Integer ' joystick id for ADDED,
            ' 						 * else instance id
            ' 						 
        End Structure


        ' Ignore private members used for padding in this struct
        ' Audio device event (event.adevice.*) 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_AudioDeviceEvent
            Public type As UInteger
            Public timestamp As UInteger
            Public which As UInteger
            Public iscapture As Byte
            Private padding1 As Byte
            Private padding2 As Byte
            Private padding3 As Byte
        End Structure


        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_TouchFingerEvent
            Public type As UInteger
            Public timestamp As UInteger
            Public touchId As Long ' SDL_TouchID
            Public fingerId As Long ' SDL_GestureID
            Public x As Single
            Public y As Single
            Public dx As Single
            Public dy As Single
            Public pressure As Single
            Public windowID As UInteger
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_MultiGestureEvent
            Public type As UInteger
            Public timestamp As UInteger
            Public touchId As Long ' SDL_TouchID
            Public dTheta As Single
            Public dDist As Single
            Public x As Single
            Public y As Single
            Public numFingers As UShort
            Public padding As UShort
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_DollarGestureEvent
            Public type As UInteger
            Public timestamp As UInteger
            Public touchId As Long ' SDL_TouchID
            Public gestureId As Long ' SDL_GestureID
            Public numFingers As UInteger
            Public [error] As Single
            Public x As Single
            Public y As Single
        End Structure


        ' File open request by system (event.drop.*), enabled by
        ' 		 * default
        ' 		 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_DropEvent
            Public type As SDL_EventType
            Public timestamp As UInteger

            ' char* filename, to be freed.
            ' 			 * Access the variable EXACTLY ONCE like this:
            ' 			 * string s = SDL.UTF8_ToManaged(evt.drop.file, true);
            ' 			 
            Public file As IntPtr
            Public windowID As UInteger
        End Structure

        ''' Cannot convert StructDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitStructDeclaration(StructDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		[System.Runtime.InteropServices.@StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        ''' 		public unsafe struct SDL_SensorEvent
        ''' 		{
        ''' 			public SDL2.SDL.SDL_EventType type;
        ''' 			public System.UInt32 timestamp;
        ''' 			public System.Int32 which;
        ''' 			public fixed float data[6];
        ''' 		}
        ''' 
        ''' 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_SensorEvent
            Public type As SDL.SDL_EventType
            Public timestamp As UInt32
            Public which As Int32
            <VBFixedArray(6)> Public data() As Single
        End Structure

        ' The "quit requested" event 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_QuitEvent
            Public type As SDL_EventType
            Public timestamp As UInteger
        End Structure


        ' A user defined event (event.user.*) 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_UserEvent
            Public type As UInteger
            Public timestamp As UInteger
            Public windowID As UInteger
            Public code As Integer
            Public data1 As IntPtr ' user-defined 
            Public data2 As IntPtr ' user-defined 
        End Structure


        ' A video driver dependent event (event.syswm.*), disabled 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_SysWMEvent
            Public type As SDL_EventType
            Public timestamp As UInteger
            Public msg As IntPtr ' SDL_SysWMmsg*, system-dependent
        End Structure

        '<StructLayout(LayoutKind.Explicit)>
        'Public Structure SDL_Event_VB
        '    Public type As SDL_EventType
        '    Public timestamp As UInt32
        '    Public eID As UInt32
        '    <VBFixedArray(64)> Public reserved() As Byte
        'End Structure


        ' General event structure 
        ' C# doesn't do unions, so we do this ugly thing. 
        <StructLayout(LayoutKind.Explicit)>
        Public Structure SDL_Event
            <FieldOffset(0)>
            Public type As SDL_EventType
            <FieldOffset(0)>
            Public display As SDL_DisplayEvent
            <FieldOffset(0)>
            Public window As SDL_WindowEvent
            <FieldOffset(0)>
            Public key As SDL_KeyboardEvent
            <FieldOffset(0)>
            Public edit As SDL.SDL_TextEditingEvent
            <FieldOffset(0)>
            Public text As SDL.SDL_TextInputEvent
            <FieldOffset(0)>
            Public motion As SDL_MouseMotionEvent
            <FieldOffset(0)>
            Public button As SDL_MouseButtonEvent
            <FieldOffset(0)>
            Public wheel As SDL_MouseWheelEvent
            <FieldOffset(0)>
            Public jaxis As SDL_JoyAxisEvent
            <FieldOffset(0)>
            Public jball As SDL_JoyBallEvent
            <FieldOffset(0)>
            Public jhat As SDL_JoyHatEvent
            <FieldOffset(0)>
            Public jbutton As SDL_JoyButtonEvent
            <FieldOffset(0)>
            Public jdevice As SDL_JoyDeviceEvent
            <FieldOffset(0)>
            Public caxis As SDL_ControllerAxisEvent
            <FieldOffset(0)>
            Public cbutton As SDL_ControllerButtonEvent
            <FieldOffset(0)>
            Public cdevice As SDL_ControllerDeviceEvent
            <FieldOffset(0)>
            Public adevice As SDL_AudioDeviceEvent
            <FieldOffset(0)>
            Public sensor As SDL.SDL_SensorEvent
            <FieldOffset(0)>
            Public quit As SDL_QuitEvent
            <FieldOffset(0)>
            Public user As SDL_UserEvent
            <FieldOffset(0)>
            Public syswm As SDL_SysWMEvent
            <FieldOffset(0)>
            Public tfinger As SDL_TouchFingerEvent
            <FieldOffset(0)>
            Public mgesture As SDL_MultiGestureEvent
            <FieldOffset(0)>
            Public dgesture As SDL_DollarGestureEvent
            <FieldOffset(0)>
            Public drop As SDL_DropEvent
        End Structure

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function SDL_EventFilter(ByVal userdata As IntPtr, ByVal sdlevent As IntPtr) As Integer ' void*
        ' SDL_Event* event, lolC#

        ' Pump the event loop, getting events from the input devices
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_PumpEvents()
        End Sub

        Public Enum SDL_eventaction
            SDL_ADDEVENT
            SDL_PEEKEVENT
            SDL_GETEVENT
        End Enum

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_PeepEvents(
        <Out> ByVal events As SDL_Event(), ByVal numevents As Integer, ByVal action As SDL_eventaction, ByVal minType As SDL_EventType, ByVal maxType As SDL_EventType) As Integer
        End Function


        ' Checks to see if certain events are in the event queue 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HasEvent(ByVal type As SDL_EventType) As SDL_bool
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HasEvents(ByVal minType As SDL_EventType, ByVal maxType As SDL_EventType) As SDL_bool
        End Function


        ' Clears events from the event queue 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_FlushEvent(ByVal type As SDL_EventType)
        End Sub

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_FlushEvents(ByVal min As SDL_EventType, ByVal max As SDL_EventType)
        End Sub


        ' Polls for currently pending events 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_PollEvent(ByRef _event As SDL_Event) As Integer '<Out> ByRef
        End Function

        ' Waits indefinitely for the next event 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_WaitEvent(<Out> ByRef _event As SDL_Event) As Integer
        End Function


        ' Waits until the specified timeout (in ms) for the next event
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_WaitEventTimeout(<Out> ByRef _event As SDL_Event, ByVal timeout As Integer) As Integer
        End Function


        ' Add an event to the event queue 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_PushEvent(ByRef _event As SDL_Event) As Integer
        End Function


        ' userdata refers to a void* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_SetEventFilter(ByVal filter As SDL_EventFilter, ByVal userdata As IntPtr)
        End Sub


        ' userdata refers to a void* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Private Function SDL_GetEventFilter(<Out> ByRef filter As IntPtr, <Out> ByRef userdata As IntPtr) As SDL_bool
        End Function

        Public Function SDL_GetEventFilter(<Out> ByRef filter As SDL_EventFilter, <Out> ByRef userdata As IntPtr) As SDL_bool
            Dim result As IntPtr = IntPtr.Zero
            Dim retval As SDL_bool = SDL_GetEventFilter(result, userdata)

            If result <> IntPtr.Zero Then
                filter = CType(Marshal.GetDelegateForFunctionPointer(result, GetType(SDL_EventFilter)), SDL_EventFilter)
            Else
                filter = Nothing
            End If

            Return retval
        End Function


        ' userdata refers to a void* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_AddEventWatch(ByVal filter As SDL_EventFilter, ByVal userdata As IntPtr)
        End Sub


        ' userdata refers to a void* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_DelEventWatch(ByVal filter As SDL_EventFilter, ByVal userdata As IntPtr)
        End Sub


        ' userdata refers to a void* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_FilterEvents(ByVal filter As SDL_EventFilter, ByVal userdata As IntPtr)
        End Sub


        ' These are for SDL_EventState() 
        Public Const SDL_QUERY As Integer = -1
        Public Const SDL_IGNORE As Integer = 0
        Public Const SDL_DISABLE As Integer = 0
        Public Const SDL_ENABLE As Integer = 1


        ' This function allows you to enable/disable certain events 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_EventState(ByVal type As SDL_EventType, ByVal state As Integer) As Byte
        End Function


        ' Get the state of an event 
        Public Function SDL_GetEventState(ByVal type As SDL_EventType) As Byte
            Return SDL_EventState(type, SDL_QUERY)
        End Function


        ' Allocate a set of user-defined events 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RegisterEvents(ByVal numevents As Integer) As UInteger
        End Function

#End Region

#Region "SDL_scancode.h"

        ' Scancodes based off USB keyboard page (0x07) 
        Public Enum SDL_Scancode
            SDL_SCANCODE_UNKNOWN = 0
            SDL_SCANCODE_A = 4
            SDL_SCANCODE_B = 5
            SDL_SCANCODE_C = 6
            SDL_SCANCODE_D = 7
            SDL_SCANCODE_E = 8
            SDL_SCANCODE_F = 9
            SDL_SCANCODE_G = 10
            SDL_SCANCODE_H = 11
            SDL_SCANCODE_I = 12
            SDL_SCANCODE_J = 13
            SDL_SCANCODE_K = 14
            SDL_SCANCODE_L = 15
            SDL_SCANCODE_M = 16
            SDL_SCANCODE_N = 17
            SDL_SCANCODE_O = 18
            SDL_SCANCODE_P = 19
            SDL_SCANCODE_Q = 20
            SDL_SCANCODE_R = 21
            SDL_SCANCODE_S = 22
            SDL_SCANCODE_T = 23
            SDL_SCANCODE_U = 24
            SDL_SCANCODE_V = 25
            SDL_SCANCODE_W = 26
            SDL_SCANCODE_X = 27
            SDL_SCANCODE_Y = 28
            SDL_SCANCODE_Z = 29
            SDL_SCANCODE_1 = 30
            SDL_SCANCODE_2 = 31
            SDL_SCANCODE_3 = 32
            SDL_SCANCODE_4 = 33
            SDL_SCANCODE_5 = 34
            SDL_SCANCODE_6 = 35
            SDL_SCANCODE_7 = 36
            SDL_SCANCODE_8 = 37
            SDL_SCANCODE_9 = 38
            SDL_SCANCODE_0 = 39
            SDL_SCANCODE_RETURN = 40
            SDL_SCANCODE_ESCAPE = 41
            SDL_SCANCODE_BACKSPACE = 42
            SDL_SCANCODE_TAB = 43
            SDL_SCANCODE_SPACE = 44
            SDL_SCANCODE_MINUS = 45
            SDL_SCANCODE_EQUALS = 46
            SDL_SCANCODE_LEFTBRACKET = 47
            SDL_SCANCODE_RIGHTBRACKET = 48
            SDL_SCANCODE_BACKSLASH = 49
            SDL_SCANCODE_NONUSHASH = 50
            SDL_SCANCODE_SEMICOLON = 51
            SDL_SCANCODE_APOSTROPHE = 52
            SDL_SCANCODE_GRAVE = 53
            SDL_SCANCODE_COMMA = 54
            SDL_SCANCODE_PERIOD = 55
            SDL_SCANCODE_SLASH = 56
            SDL_SCANCODE_CAPSLOCK = 57
            SDL_SCANCODE_F1 = 58
            SDL_SCANCODE_F2 = 59
            SDL_SCANCODE_F3 = 60
            SDL_SCANCODE_F4 = 61
            SDL_SCANCODE_F5 = 62
            SDL_SCANCODE_F6 = 63
            SDL_SCANCODE_F7 = 64
            SDL_SCANCODE_F8 = 65
            SDL_SCANCODE_F9 = 66
            SDL_SCANCODE_F10 = 67
            SDL_SCANCODE_F11 = 68
            SDL_SCANCODE_F12 = 69
            SDL_SCANCODE_PRINTSCREEN = 70
            SDL_SCANCODE_SCROLLLOCK = 71
            SDL_SCANCODE_PAUSE = 72
            SDL_SCANCODE_INSERT = 73
            SDL_SCANCODE_HOME = 74
            SDL_SCANCODE_PAGEUP = 75
            SDL_SCANCODE_DELETE = 76
            SDL_SCANCODE_END = 77
            SDL_SCANCODE_PAGEDOWN = 78
            SDL_SCANCODE_RIGHT = 79
            SDL_SCANCODE_LEFT = 80
            SDL_SCANCODE_DOWN = 81
            SDL_SCANCODE_UP = 82
            SDL_SCANCODE_NUMLOCKCLEAR = 83
            SDL_SCANCODE_KP_DIVIDE = 84
            SDL_SCANCODE_KP_MULTIPLY = 85
            SDL_SCANCODE_KP_MINUS = 86
            SDL_SCANCODE_KP_PLUS = 87
            SDL_SCANCODE_KP_ENTER = 88
            SDL_SCANCODE_KP_1 = 89
            SDL_SCANCODE_KP_2 = 90
            SDL_SCANCODE_KP_3 = 91
            SDL_SCANCODE_KP_4 = 92
            SDL_SCANCODE_KP_5 = 93
            SDL_SCANCODE_KP_6 = 94
            SDL_SCANCODE_KP_7 = 95
            SDL_SCANCODE_KP_8 = 96
            SDL_SCANCODE_KP_9 = 97
            SDL_SCANCODE_KP_0 = 98
            SDL_SCANCODE_KP_PERIOD = 99
            SDL_SCANCODE_NONUSBACKSLASH = 100
            SDL_SCANCODE_APPLICATION = 101
            SDL_SCANCODE_POWER = 102
            SDL_SCANCODE_KP_EQUALS = 103
            SDL_SCANCODE_F13 = 104
            SDL_SCANCODE_F14 = 105
            SDL_SCANCODE_F15 = 106
            SDL_SCANCODE_F16 = 107
            SDL_SCANCODE_F17 = 108
            SDL_SCANCODE_F18 = 109
            SDL_SCANCODE_F19 = 110
            SDL_SCANCODE_F20 = 111
            SDL_SCANCODE_F21 = 112
            SDL_SCANCODE_F22 = 113
            SDL_SCANCODE_F23 = 114
            SDL_SCANCODE_F24 = 115
            SDL_SCANCODE_EXECUTE = 116
            SDL_SCANCODE_HELP = 117
            SDL_SCANCODE_MENU = 118
            SDL_SCANCODE_SELECT = 119
            SDL_SCANCODE_STOP = 120
            SDL_SCANCODE_AGAIN = 121
            SDL_SCANCODE_UNDO = 122
            SDL_SCANCODE_CUT = 123
            SDL_SCANCODE_COPY = 124
            SDL_SCANCODE_PASTE = 125
            SDL_SCANCODE_FIND = 126
            SDL_SCANCODE_MUTE = 127
            SDL_SCANCODE_VOLUMEUP = 128
            SDL_SCANCODE_VOLUMEDOWN = 129
            ' not sure whether there's a reason to enable these 
            ' 	SDL_SCANCODE_LOCKINGCAPSLOCK = 130, 
            ' 	SDL_SCANCODE_LOCKINGNUMLOCK = 131, 
            ' 	SDL_SCANCODE_LOCKINGSCROLLLOCK = 132, 
            SDL_SCANCODE_KP_COMMA = 133
            SDL_SCANCODE_KP_EQUALSAS400 = 134
            SDL_SCANCODE_INTERNATIONAL1 = 135
            SDL_SCANCODE_INTERNATIONAL2 = 136
            SDL_SCANCODE_INTERNATIONAL3 = 137
            SDL_SCANCODE_INTERNATIONAL4 = 138
            SDL_SCANCODE_INTERNATIONAL5 = 139
            SDL_SCANCODE_INTERNATIONAL6 = 140
            SDL_SCANCODE_INTERNATIONAL7 = 141
            SDL_SCANCODE_INTERNATIONAL8 = 142
            SDL_SCANCODE_INTERNATIONAL9 = 143
            SDL_SCANCODE_LANG1 = 144
            SDL_SCANCODE_LANG2 = 145
            SDL_SCANCODE_LANG3 = 146
            SDL_SCANCODE_LANG4 = 147
            SDL_SCANCODE_LANG5 = 148
            SDL_SCANCODE_LANG6 = 149
            SDL_SCANCODE_LANG7 = 150
            SDL_SCANCODE_LANG8 = 151
            SDL_SCANCODE_LANG9 = 152
            SDL_SCANCODE_ALTERASE = 153
            SDL_SCANCODE_SYSREQ = 154
            SDL_SCANCODE_CANCEL = 155
            SDL_SCANCODE_CLEAR = 156
            SDL_SCANCODE_PRIOR = 157
            SDL_SCANCODE_RETURN2 = 158
            SDL_SCANCODE_SEPARATOR = 159
            SDL_SCANCODE_OUT = 160
            SDL_SCANCODE_OPER = 161
            SDL_SCANCODE_CLEARAGAIN = 162
            SDL_SCANCODE_CRSEL = 163
            SDL_SCANCODE_EXSEL = 164
            SDL_SCANCODE_KP_00 = 176
            SDL_SCANCODE_KP_000 = 177
            SDL_SCANCODE_THOUSANDSSEPARATOR = 178
            SDL_SCANCODE_DECIMALSEPARATOR = 179
            SDL_SCANCODE_CURRENCYUNIT = 180
            SDL_SCANCODE_CURRENCYSUBUNIT = 181
            SDL_SCANCODE_KP_LEFTPAREN = 182
            SDL_SCANCODE_KP_RIGHTPAREN = 183
            SDL_SCANCODE_KP_LEFTBRACE = 184
            SDL_SCANCODE_KP_RIGHTBRACE = 185
            SDL_SCANCODE_KP_TAB = 186
            SDL_SCANCODE_KP_BACKSPACE = 187
            SDL_SCANCODE_KP_A = 188
            SDL_SCANCODE_KP_B = 189
            SDL_SCANCODE_KP_C = 190
            SDL_SCANCODE_KP_D = 191
            SDL_SCANCODE_KP_E = 192
            SDL_SCANCODE_KP_F = 193
            SDL_SCANCODE_KP_XOR = 194
            SDL_SCANCODE_KP_POWER = 195
            SDL_SCANCODE_KP_PERCENT = 196
            SDL_SCANCODE_KP_LESS = 197
            SDL_SCANCODE_KP_GREATER = 198
            SDL_SCANCODE_KP_AMPERSAND = 199
            SDL_SCANCODE_KP_DBLAMPERSAND = 200
            SDL_SCANCODE_KP_VERTICALBAR = 201
            SDL_SCANCODE_KP_DBLVERTICALBAR = 202
            SDL_SCANCODE_KP_COLON = 203
            SDL_SCANCODE_KP_HASH = 204
            SDL_SCANCODE_KP_SPACE = 205
            SDL_SCANCODE_KP_AT = 206
            SDL_SCANCODE_KP_EXCLAM = 207
            SDL_SCANCODE_KP_MEMSTORE = 208
            SDL_SCANCODE_KP_MEMRECALL = 209
            SDL_SCANCODE_KP_MEMCLEAR = 210
            SDL_SCANCODE_KP_MEMADD = 211
            SDL_SCANCODE_KP_MEMSUBTRACT = 212
            SDL_SCANCODE_KP_MEMMULTIPLY = 213
            SDL_SCANCODE_KP_MEMDIVIDE = 214
            SDL_SCANCODE_KP_PLUSMINUS = 215
            SDL_SCANCODE_KP_CLEAR = 216
            SDL_SCANCODE_KP_CLEARENTRY = 217
            SDL_SCANCODE_KP_BINARY = 218
            SDL_SCANCODE_KP_OCTAL = 219
            SDL_SCANCODE_KP_DECIMAL = 220
            SDL_SCANCODE_KP_HEXADECIMAL = 221
            SDL_SCANCODE_LCTRL = 224
            SDL_SCANCODE_LSHIFT = 225
            SDL_SCANCODE_LALT = 226
            SDL_SCANCODE_LGUI = 227
            SDL_SCANCODE_RCTRL = 228
            SDL_SCANCODE_RSHIFT = 229
            SDL_SCANCODE_RALT = 230
            SDL_SCANCODE_RGUI = 231
            SDL_SCANCODE_MODE = 257

            ' These come from the USB consumer page (0x0C) 
            SDL_SCANCODE_AUDIONEXT = 258
            SDL_SCANCODE_AUDIOPREV = 259
            SDL_SCANCODE_AUDIOSTOP = 260
            SDL_SCANCODE_AUDIOPLAY = 261
            SDL_SCANCODE_AUDIOMUTE = 262
            SDL_SCANCODE_MEDIASELECT = 263
            SDL_SCANCODE_WWW = 264
            SDL_SCANCODE_MAIL = 265
            SDL_SCANCODE_CALCULATOR = 266
            SDL_SCANCODE_COMPUTER = 267
            SDL_SCANCODE_AC_SEARCH = 268
            SDL_SCANCODE_AC_HOME = 269
            SDL_SCANCODE_AC_BACK = 270
            SDL_SCANCODE_AC_FORWARD = 271
            SDL_SCANCODE_AC_STOP = 272
            SDL_SCANCODE_AC_REFRESH = 273
            SDL_SCANCODE_AC_BOOKMARKS = 274

            ' These come from other sources, and are mostly mac related 
            SDL_SCANCODE_BRIGHTNESSDOWN = 275
            SDL_SCANCODE_BRIGHTNESSUP = 276
            SDL_SCANCODE_DISPLAYSWITCH = 277
            SDL_SCANCODE_KBDILLUMTOGGLE = 278
            SDL_SCANCODE_KBDILLUMDOWN = 279
            SDL_SCANCODE_KBDILLUMUP = 280
            SDL_SCANCODE_EJECT = 281
            SDL_SCANCODE_SLEEP = 282
            SDL_SCANCODE_APP1 = 283
            SDL_SCANCODE_APP2 = 284

            ' These come from the USB consumer page (0x0C) 
            SDL_SCANCODE_AUDIOREWIND = 285
            SDL_SCANCODE_AUDIOFASTFORWARD = 286

            ' This is not a key, simply marks the number of scancodes
            ' 			 * so that you know how big to make your arrays. 
            SDL_NUM_SCANCODES = 512
        End Enum


#End Region

#Region "SDL_keycode.h"

        Public Const SDLK_SCANCODE_MASK As Integer = 1 << 30

        Public Function SDL_SCANCODE_TO_KEYCODE(ByVal X As SDL_Scancode) As SDL_Keycode
            Return X Or SDLK_SCANCODE_MASK
        End Function

        Public Enum SDL_Keycode
            SDLK_UNKNOWN = 0
            SDLK_RETURN = 13 'ChrW(13) 'Enter Key
            SDLK_ESCAPE = 27 ' '\033' 'Escape key
            SDLK_BACKSPACE = 8 ' ChrW(8)
            SDLK_TAB = 9 'ChrW(9)
            SDLK_SPACE = 32 ' (" ") Spacebar
            SDLK_EXCLAIM = 33 ' ("!")
            SDLK_QUOTEDBL = 34 ' """"c
            SDLK_HASH = 35 '"#"c
            SDLK_PERCENT = 37 ' "%"c
            SDLK_DOLLAR = 36 '"$"c
            SDLK_AMPERSAND = 38 '"&"c mac value..
            SDLK_QUOTE = 192 '"'"c 'unsure
            SDLK_LEFTPAREN = 40 ' "("c
            SDLK_RIGHTPAREN = 41 '")"c
            SDLK_ASTERISK = 106 ' "*"c
            SDLK_PLUS = 107 ' "+"c '''''
            SDLK_COMMA = 188 ' ","c
            SDLK_MINUS = 189 ' "-"c
            SDLK_PERIOD = 109 ' "."c
            SDLK_SLASH = 191 ' "/"c

            SDLK_0 = 96 ' "0"c
            SDLK_1 = 97 ' "1"c
            SDLK_2 = 98 ' "2"c
            SDLK_3 = 99 ' "3"c
            SDLK_4 = 100 ' "4"c
            SDLK_5 = 101 ' "5"c
            SDLK_6 = 102 '"6"c
            SDLK_7 = 103 ' "7"c
            SDLK_8 = 104 ' "8"c
            SDLK_9 = 105 '"9"c

            SDLK_COLON = 58 '":"c
            SDLK_SEMICOLON = 186 '";"c
            SDLK_LESS = 60 '"<"c
            SDLK_EQUALS = 61 ' "="c
            SDLK_GREATER = 62 ' ">"c
            SDLK_QUESTION = 63 ' "?"c
            SDLK_AT = 64 ' "@"c
            ' 
            ' 			Skip uppercase letters
            ' 			
            SDLK_LEFTBRACKET = 219 '"["c
            SDLK_BACKSLASH = 220 '"\"c
            SDLK_RIGHTBRACKET = 221 ' "]"c
            SDLK_CARET = 94 ' "^"c
            SDLK_UNDERSCORE = 95 ' "_"c
            SDLK_BACKQUOTE = 222 '"`"c 'this may be reversed with the other quote

            SDLK_a = 65 ' "a"c
            SDLK_b = 66 ' "b"c
            SDLK_c = 67 ' "c"c
            SDLK_d = 68 '"d"c
            SDLK_e = 69 ' "e"c
            SDLK_f = 70 ' "f"c
            SDLK_g = 71 '"g"c
            SDLK_h = 72 ' "h"c
            SDLK_i = 73 '"i"c
            SDLK_j = 74 ' "j"c
            SDLK_k = 75 '"k"c
            SDLK_l = 76 '"l"c
            SDLK_m = 77 '"m"c
            SDLK_n = 78 ' "n"c
            SDLK_o = 79 ' "o"c
            SDLK_p = 80 ' "p"c
            SDLK_q = 81 '"q"c
            SDLK_r = 82 '"r"c
            SDLK_s = 83 '"s"c
            SDLK_t = 84 ' "t"c
            SDLK_u = 85 '"u"c
            SDLK_v = 86 ' "v"c
            SDLK_w = 87 ' "w"c
            SDLK_x = 88 ' "x"c
            SDLK_y = 89 ' "y"c
            SDLK_z = 90 ' "z"c

            SDLK_CAPSLOCK = SDL_Scancode.SDL_SCANCODE_CAPSLOCK Or SDLK_SCANCODE_MASK
            SDLK_F1 = SDL_Scancode.SDL_SCANCODE_F1 Or SDLK_SCANCODE_MASK
            SDLK_F2 = SDL_Scancode.SDL_SCANCODE_F2 Or SDLK_SCANCODE_MASK
            SDLK_F3 = SDL_Scancode.SDL_SCANCODE_F3 Or SDLK_SCANCODE_MASK
            SDLK_F4 = SDL_Scancode.SDL_SCANCODE_F4 Or SDLK_SCANCODE_MASK
            SDLK_F5 = SDL_Scancode.SDL_SCANCODE_F5 Or SDLK_SCANCODE_MASK
            SDLK_F6 = SDL_Scancode.SDL_SCANCODE_F6 Or SDLK_SCANCODE_MASK
            SDLK_F7 = SDL_Scancode.SDL_SCANCODE_F7 Or SDLK_SCANCODE_MASK
            SDLK_F8 = SDL_Scancode.SDL_SCANCODE_F8 Or SDLK_SCANCODE_MASK
            SDLK_F9 = SDL_Scancode.SDL_SCANCODE_F9 Or SDLK_SCANCODE_MASK
            SDLK_F10 = SDL_Scancode.SDL_SCANCODE_F10 Or SDLK_SCANCODE_MASK
            SDLK_F11 = SDL_Scancode.SDL_SCANCODE_F11 Or SDLK_SCANCODE_MASK
            SDLK_F12 = SDL_Scancode.SDL_SCANCODE_F12 Or SDLK_SCANCODE_MASK
            SDLK_PRINTSCREEN = SDL_Scancode.SDL_SCANCODE_PRINTSCREEN Or SDLK_SCANCODE_MASK
            SDLK_SCROLLLOCK = SDL_Scancode.SDL_SCANCODE_SCROLLLOCK Or SDLK_SCANCODE_MASK
            SDLK_PAUSE = SDL_Scancode.SDL_SCANCODE_PAUSE Or SDLK_SCANCODE_MASK
            SDLK_INSERT = SDL_Scancode.SDL_SCANCODE_INSERT Or SDLK_SCANCODE_MASK
            SDLK_HOME = SDL_Scancode.SDL_SCANCODE_HOME Or SDLK_SCANCODE_MASK
            SDLK_PAGEUP = SDL_Scancode.SDL_SCANCODE_PAGEUP Or SDLK_SCANCODE_MASK
            SDLK_DELETE = 127
            SDLK_END = SDL_Scancode.SDL_SCANCODE_END Or SDLK_SCANCODE_MASK
            SDLK_PAGEDOWN = SDL_Scancode.SDL_SCANCODE_PAGEDOWN Or SDLK_SCANCODE_MASK
            SDLK_RIGHT = SDL_Scancode.SDL_SCANCODE_RIGHT Or SDLK_SCANCODE_MASK
            SDLK_LEFT = SDL_Scancode.SDL_SCANCODE_LEFT Or SDLK_SCANCODE_MASK
            SDLK_DOWN = SDL_Scancode.SDL_SCANCODE_DOWN Or SDLK_SCANCODE_MASK
            SDLK_UP = SDL_Scancode.SDL_SCANCODE_UP Or SDLK_SCANCODE_MASK
            SDLK_NUMLOCKCLEAR = SDL_Scancode.SDL_SCANCODE_NUMLOCKCLEAR Or SDLK_SCANCODE_MASK
            SDLK_KP_DIVIDE = SDL_Scancode.SDL_SCANCODE_KP_DIVIDE Or SDLK_SCANCODE_MASK
            SDLK_KP_MULTIPLY = SDL_Scancode.SDL_SCANCODE_KP_MULTIPLY Or SDLK_SCANCODE_MASK
            SDLK_KP_MINUS = SDL_Scancode.SDL_SCANCODE_KP_MINUS Or SDLK_SCANCODE_MASK
            SDLK_KP_PLUS = SDL_Scancode.SDL_SCANCODE_KP_PLUS Or SDLK_SCANCODE_MASK
            SDLK_KP_ENTER = SDL_Scancode.SDL_SCANCODE_KP_ENTER Or SDLK_SCANCODE_MASK
            SDLK_KP_1 = SDL_Scancode.SDL_SCANCODE_KP_1 Or SDLK_SCANCODE_MASK
            SDLK_KP_2 = SDL_Scancode.SDL_SCANCODE_KP_2 Or SDLK_SCANCODE_MASK
            SDLK_KP_3 = SDL_Scancode.SDL_SCANCODE_KP_3 Or SDLK_SCANCODE_MASK
            SDLK_KP_4 = SDL_Scancode.SDL_SCANCODE_KP_4 Or SDLK_SCANCODE_MASK
            SDLK_KP_5 = SDL_Scancode.SDL_SCANCODE_KP_5 Or SDLK_SCANCODE_MASK
            SDLK_KP_6 = SDL_Scancode.SDL_SCANCODE_KP_6 Or SDLK_SCANCODE_MASK
            SDLK_KP_7 = SDL_Scancode.SDL_SCANCODE_KP_7 Or SDLK_SCANCODE_MASK
            SDLK_KP_8 = SDL_Scancode.SDL_SCANCODE_KP_8 Or SDLK_SCANCODE_MASK
            SDLK_KP_9 = SDL_Scancode.SDL_SCANCODE_KP_9 Or SDLK_SCANCODE_MASK
            SDLK_KP_0 = SDL_Scancode.SDL_SCANCODE_KP_0 Or SDLK_SCANCODE_MASK
            SDLK_KP_PERIOD = SDL_Scancode.SDL_SCANCODE_KP_PERIOD Or SDLK_SCANCODE_MASK
            SDLK_APPLICATION = SDL_Scancode.SDL_SCANCODE_APPLICATION Or SDLK_SCANCODE_MASK
            SDLK_POWER = SDL_Scancode.SDL_SCANCODE_POWER Or SDLK_SCANCODE_MASK
            SDLK_KP_EQUALS = SDL_Scancode.SDL_SCANCODE_KP_EQUALS Or SDLK_SCANCODE_MASK
            SDLK_F13 = SDL_Scancode.SDL_SCANCODE_F13 Or SDLK_SCANCODE_MASK
            SDLK_F14 = SDL_Scancode.SDL_SCANCODE_F14 Or SDLK_SCANCODE_MASK
            SDLK_F15 = SDL_Scancode.SDL_SCANCODE_F15 Or SDLK_SCANCODE_MASK
            SDLK_F16 = SDL_Scancode.SDL_SCANCODE_F16 Or SDLK_SCANCODE_MASK
            SDLK_F17 = SDL_Scancode.SDL_SCANCODE_F17 Or SDLK_SCANCODE_MASK
            SDLK_F18 = SDL_Scancode.SDL_SCANCODE_F18 Or SDLK_SCANCODE_MASK
            SDLK_F19 = SDL_Scancode.SDL_SCANCODE_F19 Or SDLK_SCANCODE_MASK
            SDLK_F20 = SDL_Scancode.SDL_SCANCODE_F20 Or SDLK_SCANCODE_MASK
            SDLK_F21 = SDL_Scancode.SDL_SCANCODE_F21 Or SDLK_SCANCODE_MASK
            SDLK_F22 = SDL_Scancode.SDL_SCANCODE_F22 Or SDLK_SCANCODE_MASK
            SDLK_F23 = SDL_Scancode.SDL_SCANCODE_F23 Or SDLK_SCANCODE_MASK
            SDLK_F24 = SDL_Scancode.SDL_SCANCODE_F24 Or SDLK_SCANCODE_MASK
            SDLK_EXECUTE = SDL_Scancode.SDL_SCANCODE_EXECUTE Or SDLK_SCANCODE_MASK
            SDLK_HELP = SDL_Scancode.SDL_SCANCODE_HELP Or SDLK_SCANCODE_MASK
            SDLK_MENU = SDL_Scancode.SDL_SCANCODE_MENU Or SDLK_SCANCODE_MASK
            SDLK_SELECT = SDL_Scancode.SDL_SCANCODE_SELECT Or SDLK_SCANCODE_MASK
            SDLK_STOP = SDL_Scancode.SDL_SCANCODE_STOP Or SDLK_SCANCODE_MASK
            SDLK_AGAIN = SDL_Scancode.SDL_SCANCODE_AGAIN Or SDLK_SCANCODE_MASK
            SDLK_UNDO = SDL_Scancode.SDL_SCANCODE_UNDO Or SDLK_SCANCODE_MASK
            SDLK_CUT = SDL_Scancode.SDL_SCANCODE_CUT Or SDLK_SCANCODE_MASK
            SDLK_COPY = SDL_Scancode.SDL_SCANCODE_COPY Or SDLK_SCANCODE_MASK
            SDLK_PASTE = SDL_Scancode.SDL_SCANCODE_PASTE Or SDLK_SCANCODE_MASK
            SDLK_FIND = SDL_Scancode.SDL_SCANCODE_FIND Or SDLK_SCANCODE_MASK
            SDLK_MUTE = SDL_Scancode.SDL_SCANCODE_MUTE Or SDLK_SCANCODE_MASK
            SDLK_VOLUMEUP = SDL_Scancode.SDL_SCANCODE_VOLUMEUP Or SDLK_SCANCODE_MASK
            SDLK_VOLUMEDOWN = SDL_Scancode.SDL_SCANCODE_VOLUMEDOWN Or SDLK_SCANCODE_MASK
            SDLK_KP_COMMA = SDL_Scancode.SDL_SCANCODE_KP_COMMA Or SDLK_SCANCODE_MASK
            SDLK_KP_EQUALSAS400 = SDL_Scancode.SDL_SCANCODE_KP_EQUALSAS400 Or SDLK_SCANCODE_MASK
            SDLK_ALTERASE = SDL_Scancode.SDL_SCANCODE_ALTERASE Or SDLK_SCANCODE_MASK
            SDLK_SYSREQ = SDL_Scancode.SDL_SCANCODE_SYSREQ Or SDLK_SCANCODE_MASK
            SDLK_CANCEL = SDL_Scancode.SDL_SCANCODE_CANCEL Or SDLK_SCANCODE_MASK
            SDLK_CLEAR = SDL_Scancode.SDL_SCANCODE_CLEAR Or SDLK_SCANCODE_MASK
            SDLK_PRIOR = SDL_Scancode.SDL_SCANCODE_PRIOR Or SDLK_SCANCODE_MASK
            SDLK_RETURN2 = SDL_Scancode.SDL_SCANCODE_RETURN2 Or SDLK_SCANCODE_MASK
            SDLK_SEPARATOR = SDL_Scancode.SDL_SCANCODE_SEPARATOR Or SDLK_SCANCODE_MASK
            SDLK_OUT = SDL_Scancode.SDL_SCANCODE_OUT Or SDLK_SCANCODE_MASK
            SDLK_OPER = SDL_Scancode.SDL_SCANCODE_OPER Or SDLK_SCANCODE_MASK
            SDLK_CLEARAGAIN = SDL_Scancode.SDL_SCANCODE_CLEARAGAIN Or SDLK_SCANCODE_MASK
            SDLK_CRSEL = SDL_Scancode.SDL_SCANCODE_CRSEL Or SDLK_SCANCODE_MASK
            SDLK_EXSEL = SDL_Scancode.SDL_SCANCODE_EXSEL Or SDLK_SCANCODE_MASK
            SDLK_KP_00 = SDL_Scancode.SDL_SCANCODE_KP_00 Or SDLK_SCANCODE_MASK
            SDLK_KP_000 = SDL_Scancode.SDL_SCANCODE_KP_000 Or SDLK_SCANCODE_MASK
            SDLK_THOUSANDSSEPARATOR = SDL_Scancode.SDL_SCANCODE_THOUSANDSSEPARATOR Or SDLK_SCANCODE_MASK
            SDLK_DECIMALSEPARATOR = SDL_Scancode.SDL_SCANCODE_DECIMALSEPARATOR Or SDLK_SCANCODE_MASK
            SDLK_CURRENCYUNIT = SDL_Scancode.SDL_SCANCODE_CURRENCYUNIT Or SDLK_SCANCODE_MASK
            SDLK_CURRENCYSUBUNIT = SDL_Scancode.SDL_SCANCODE_CURRENCYSUBUNIT Or SDLK_SCANCODE_MASK
            SDLK_KP_LEFTPAREN = SDL_Scancode.SDL_SCANCODE_KP_LEFTPAREN Or SDLK_SCANCODE_MASK
            SDLK_KP_RIGHTPAREN = SDL_Scancode.SDL_SCANCODE_KP_RIGHTPAREN Or SDLK_SCANCODE_MASK
            SDLK_KP_LEFTBRACE = SDL_Scancode.SDL_SCANCODE_KP_LEFTBRACE Or SDLK_SCANCODE_MASK
            SDLK_KP_RIGHTBRACE = SDL_Scancode.SDL_SCANCODE_KP_RIGHTBRACE Or SDLK_SCANCODE_MASK
            SDLK_KP_TAB = SDL_Scancode.SDL_SCANCODE_KP_TAB Or SDLK_SCANCODE_MASK
            SDLK_KP_BACKSPACE = SDL_Scancode.SDL_SCANCODE_KP_BACKSPACE Or SDLK_SCANCODE_MASK
            SDLK_KP_A = SDL_Scancode.SDL_SCANCODE_KP_A Or SDLK_SCANCODE_MASK
            SDLK_KP_B = SDL_Scancode.SDL_SCANCODE_KP_B Or SDLK_SCANCODE_MASK
            SDLK_KP_C = SDL_Scancode.SDL_SCANCODE_KP_C Or SDLK_SCANCODE_MASK
            SDLK_KP_D = SDL_Scancode.SDL_SCANCODE_KP_D Or SDLK_SCANCODE_MASK
            SDLK_KP_E = SDL_Scancode.SDL_SCANCODE_KP_E Or SDLK_SCANCODE_MASK
            SDLK_KP_F = SDL_Scancode.SDL_SCANCODE_KP_F Or SDLK_SCANCODE_MASK
            SDLK_KP_XOR = SDL_Scancode.SDL_SCANCODE_KP_XOR Or SDLK_SCANCODE_MASK
            SDLK_KP_POWER = SDL_Scancode.SDL_SCANCODE_KP_POWER Or SDLK_SCANCODE_MASK
            SDLK_KP_PERCENT = SDL_Scancode.SDL_SCANCODE_KP_PERCENT Or SDLK_SCANCODE_MASK
            SDLK_KP_LESS = SDL_Scancode.SDL_SCANCODE_KP_LESS Or SDLK_SCANCODE_MASK
            SDLK_KP_GREATER = SDL_Scancode.SDL_SCANCODE_KP_GREATER Or SDLK_SCANCODE_MASK
            SDLK_KP_AMPERSAND = SDL_Scancode.SDL_SCANCODE_KP_AMPERSAND Or SDLK_SCANCODE_MASK
            SDLK_KP_DBLAMPERSAND = SDL_Scancode.SDL_SCANCODE_KP_DBLAMPERSAND Or SDLK_SCANCODE_MASK
            SDLK_KP_VERTICALBAR = SDL_Scancode.SDL_SCANCODE_KP_VERTICALBAR Or SDLK_SCANCODE_MASK
            SDLK_KP_DBLVERTICALBAR = SDL_Scancode.SDL_SCANCODE_KP_DBLVERTICALBAR Or SDLK_SCANCODE_MASK
            SDLK_KP_COLON = SDL_Scancode.SDL_SCANCODE_KP_COLON Or SDLK_SCANCODE_MASK
            SDLK_KP_HASH = SDL_Scancode.SDL_SCANCODE_KP_HASH Or SDLK_SCANCODE_MASK
            SDLK_KP_SPACE = SDL_Scancode.SDL_SCANCODE_KP_SPACE Or SDLK_SCANCODE_MASK
            SDLK_KP_AT = SDL_Scancode.SDL_SCANCODE_KP_AT Or SDLK_SCANCODE_MASK
            SDLK_KP_EXCLAM = SDL_Scancode.SDL_SCANCODE_KP_EXCLAM Or SDLK_SCANCODE_MASK
            SDLK_KP_MEMSTORE = SDL_Scancode.SDL_SCANCODE_KP_MEMSTORE Or SDLK_SCANCODE_MASK
            SDLK_KP_MEMRECALL = SDL_Scancode.SDL_SCANCODE_KP_MEMRECALL Or SDLK_SCANCODE_MASK
            SDLK_KP_MEMCLEAR = SDL_Scancode.SDL_SCANCODE_KP_MEMCLEAR Or SDLK_SCANCODE_MASK
            SDLK_KP_MEMADD = SDL_Scancode.SDL_SCANCODE_KP_MEMADD Or SDLK_SCANCODE_MASK
            SDLK_KP_MEMSUBTRACT = SDL_Scancode.SDL_SCANCODE_KP_MEMSUBTRACT Or SDLK_SCANCODE_MASK
            SDLK_KP_MEMMULTIPLY = SDL_Scancode.SDL_SCANCODE_KP_MEMMULTIPLY Or SDLK_SCANCODE_MASK
            SDLK_KP_MEMDIVIDE = SDL_Scancode.SDL_SCANCODE_KP_MEMDIVIDE Or SDLK_SCANCODE_MASK
            SDLK_KP_PLUSMINUS = SDL_Scancode.SDL_SCANCODE_KP_PLUSMINUS Or SDLK_SCANCODE_MASK
            SDLK_KP_CLEAR = SDL_Scancode.SDL_SCANCODE_KP_CLEAR Or SDLK_SCANCODE_MASK
            SDLK_KP_CLEARENTRY = SDL_Scancode.SDL_SCANCODE_KP_CLEARENTRY Or SDLK_SCANCODE_MASK
            SDLK_KP_BINARY = SDL_Scancode.SDL_SCANCODE_KP_BINARY Or SDLK_SCANCODE_MASK
            SDLK_KP_OCTAL = SDL_Scancode.SDL_SCANCODE_KP_OCTAL Or SDLK_SCANCODE_MASK
            SDLK_KP_DECIMAL = SDL_Scancode.SDL_SCANCODE_KP_DECIMAL Or SDLK_SCANCODE_MASK
            SDLK_KP_HEXADECIMAL = SDL_Scancode.SDL_SCANCODE_KP_HEXADECIMAL Or SDLK_SCANCODE_MASK
            SDLK_LCTRL = SDL_Scancode.SDL_SCANCODE_LCTRL Or SDLK_SCANCODE_MASK
            SDLK_LSHIFT = SDL_Scancode.SDL_SCANCODE_LSHIFT Or SDLK_SCANCODE_MASK
            SDLK_LALT = SDL_Scancode.SDL_SCANCODE_LALT Or SDLK_SCANCODE_MASK
            SDLK_LGUI = SDL_Scancode.SDL_SCANCODE_LGUI Or SDLK_SCANCODE_MASK
            SDLK_RCTRL = SDL_Scancode.SDL_SCANCODE_RCTRL Or SDLK_SCANCODE_MASK
            SDLK_RSHIFT = SDL_Scancode.SDL_SCANCODE_RSHIFT Or SDLK_SCANCODE_MASK
            SDLK_RALT = SDL_Scancode.SDL_SCANCODE_RALT Or SDLK_SCANCODE_MASK
            SDLK_RGUI = SDL_Scancode.SDL_SCANCODE_RGUI Or SDLK_SCANCODE_MASK
            SDLK_MODE = SDL_Scancode.SDL_SCANCODE_MODE Or SDLK_SCANCODE_MASK
            SDLK_AUDIONEXT = SDL_Scancode.SDL_SCANCODE_AUDIONEXT Or SDLK_SCANCODE_MASK
            SDLK_AUDIOPREV = SDL_Scancode.SDL_SCANCODE_AUDIOPREV Or SDLK_SCANCODE_MASK
            SDLK_AUDIOSTOP = SDL_Scancode.SDL_SCANCODE_AUDIOSTOP Or SDLK_SCANCODE_MASK
            SDLK_AUDIOPLAY = SDL_Scancode.SDL_SCANCODE_AUDIOPLAY Or SDLK_SCANCODE_MASK
            SDLK_AUDIOMUTE = SDL_Scancode.SDL_SCANCODE_AUDIOMUTE Or SDLK_SCANCODE_MASK
            SDLK_MEDIASELECT = SDL_Scancode.SDL_SCANCODE_MEDIASELECT Or SDLK_SCANCODE_MASK
            SDLK_WWW = SDL_Scancode.SDL_SCANCODE_WWW Or SDLK_SCANCODE_MASK
            SDLK_MAIL = SDL_Scancode.SDL_SCANCODE_MAIL Or SDLK_SCANCODE_MASK
            SDLK_CALCULATOR = SDL_Scancode.SDL_SCANCODE_CALCULATOR Or SDLK_SCANCODE_MASK
            SDLK_COMPUTER = SDL_Scancode.SDL_SCANCODE_COMPUTER Or SDLK_SCANCODE_MASK
            SDLK_AC_SEARCH = SDL_Scancode.SDL_SCANCODE_AC_SEARCH Or SDLK_SCANCODE_MASK
            SDLK_AC_HOME = SDL_Scancode.SDL_SCANCODE_AC_HOME Or SDLK_SCANCODE_MASK
            SDLK_AC_BACK = SDL_Scancode.SDL_SCANCODE_AC_BACK Or SDLK_SCANCODE_MASK
            SDLK_AC_FORWARD = SDL_Scancode.SDL_SCANCODE_AC_FORWARD Or SDLK_SCANCODE_MASK
            SDLK_AC_STOP = SDL_Scancode.SDL_SCANCODE_AC_STOP Or SDLK_SCANCODE_MASK
            SDLK_AC_REFRESH = SDL_Scancode.SDL_SCANCODE_AC_REFRESH Or SDLK_SCANCODE_MASK
            SDLK_AC_BOOKMARKS = SDL_Scancode.SDL_SCANCODE_AC_BOOKMARKS Or SDLK_SCANCODE_MASK
            SDLK_BRIGHTNESSDOWN = SDL_Scancode.SDL_SCANCODE_BRIGHTNESSDOWN Or SDLK_SCANCODE_MASK
            SDLK_BRIGHTNESSUP = SDL_Scancode.SDL_SCANCODE_BRIGHTNESSUP Or SDLK_SCANCODE_MASK
            SDLK_DISPLAYSWITCH = SDL_Scancode.SDL_SCANCODE_DISPLAYSWITCH Or SDLK_SCANCODE_MASK
            SDLK_KBDILLUMTOGGLE = SDL_Scancode.SDL_SCANCODE_KBDILLUMTOGGLE Or SDLK_SCANCODE_MASK
            SDLK_KBDILLUMDOWN = SDL_Scancode.SDL_SCANCODE_KBDILLUMDOWN Or SDLK_SCANCODE_MASK
            SDLK_KBDILLUMUP = SDL_Scancode.SDL_SCANCODE_KBDILLUMUP Or SDLK_SCANCODE_MASK
            SDLK_EJECT = SDL_Scancode.SDL_SCANCODE_EJECT Or SDLK_SCANCODE_MASK
            SDLK_SLEEP = SDL_Scancode.SDL_SCANCODE_SLEEP Or SDLK_SCANCODE_MASK
            SDLK_APP1 = SDL_Scancode.SDL_SCANCODE_APP1 Or SDLK_SCANCODE_MASK
            SDLK_APP2 = SDL_Scancode.SDL_SCANCODE_APP2 Or SDLK_SCANCODE_MASK
            SDLK_AUDIOREWIND = SDL_Scancode.SDL_SCANCODE_AUDIOREWIND Or SDLK_SCANCODE_MASK
            SDLK_AUDIOFASTFORWARD = SDL_Scancode.SDL_SCANCODE_AUDIOFASTFORWARD Or SDLK_SCANCODE_MASK
        End Enum


        ' Key modifiers (bitfield) 
        <Flags>
        Public Enum SDL_Keymod As UShort
            KMOD_NONE = &H0
            KMOD_LSHIFT = &H1
            KMOD_RSHIFT = &H2
            KMOD_LCTRL = &H40
            KMOD_RCTRL = &H80
            KMOD_LALT = &H100
            KMOD_RALT = &H200
            KMOD_LGUI = &H400
            KMOD_RGUI = &H800
            KMOD_NUM = &H1000
            KMOD_CAPS = &H2000
            KMOD_MODE = &H4000
            KMOD_RESERVED = &H8000

            ' These are defines in the SDL headers 
            KMOD_CTRL = KMOD_LCTRL Or KMOD_RCTRL
            KMOD_SHIFT = KMOD_LSHIFT Or KMOD_RSHIFT
            KMOD_ALT = KMOD_LALT Or KMOD_RALT
            KMOD_GUI = KMOD_LGUI Or KMOD_RGUI
        End Enum


#End Region

#Region "SDL_keyboard.h"

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_Keysym
            Public scancode As SDL_Scancode
            Public sym As SDL_Keycode
            Public [mod] As SDL_Keymod ' UInt16 
            Public unicode As UInteger ' Deprecated 
        End Structure


        ' Get the window which has kbd focus 
        ' Return type is an SDL_Window pointer 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetKeyboardFocus() As IntPtr
        End Function


        ' Get a snapshot of the keyboard state. 
        ' Return value is a pointer to a UInt8 array 
        ' Numkeys returns the size of the array if non-null 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetKeyboardState(<Out> ByRef numkeys As Integer) As IntPtr
        End Function


        ' Get the current key modifier state for the keyboard. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetModState() As SDL_Keymod
        End Function


        ' Set the current key modifier state 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_SetModState(ByVal modstate As SDL_Keymod)
        End Sub


        ' Get the key code corresponding to the given scancode
        ' 		 * with the current keyboard layout.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetKeyFromScancode(ByVal scancode As SDL_Scancode) As SDL_Keycode
        End Function


        ' Get the scancode for the given keycode 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetScancodeFromKey(ByVal key As SDL_Keycode) As SDL_Scancode
        End Function


        ' Wrapper for SDL_GetScancodeName 
        <DllImport(nativeLibName, EntryPoint:="SDL_GetScancodeName", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GetScancodeName(ByVal scancode As SDL_Scancode) As IntPtr
        End Function

        Public Function SDL_GetScancodeName(ByVal scancode As SDL_Scancode) As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GetScancodeName(scancode))

            ' Get a scancode from a human-readable name 
        End Function

        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* Get a scancode from a human-readable name */
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_GetScancodeFromName", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe SDL2.SDL.SDL_Scancode INTERNAL_SDL_GetScancodeFromName(
        ''' 			byte* name
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe SDL2.SDL.SDL_Scancode SDL_GetScancodeFromName(string name)
        ''' 		{
        ''' 			int utf8NameBufSize = SDL2.SDL.Utf8Size(name);
        ''' 			byte* utf8Name = stackalloc byte[utf8NameBufSize];
        ''' 			return SDL2.SDL.INTERNAL_SDL_GetScancodeFromName(
        ''' 				SDL2.SDL.Utf8Encode(name, utf8Name, utf8NameBufSize)
        ''' 			);
        ''' 		}
        ''' 
        ''' 

        ' Wrapper for SDL_GetKeyName 
        <DllImport(nativeLibName, EntryPoint:="SDL_GetKeyName", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GetKeyName(ByVal key As SDL_Keycode) As IntPtr
        End Function

        Public Function SDL_GetKeyName(ByVal key As SDL_Keycode) As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GetKeyName(key))

            ' Get a key code from a human-readable name 
        End Function

        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* Get a key code from a human-readable name */
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_GetKeyFromName", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe SDL2.SDL.SDL_Keycode INTERNAL_SDL_GetKeyFromName(
        ''' 			byte* name
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe SDL2.SDL.SDL_Keycode SDL_GetKeyFromName(string name)
        ''' 		{
        ''' 			int utf8NameBufSize = SDL2.SDL.Utf8Size(name);
        ''' 			byte* utf8Name = stackalloc byte[utf8NameBufSize];
        ''' 			return SDL2.SDL.INTERNAL_SDL_GetKeyFromName(
        ''' 				SDL2.SDL.Utf8Encode(name, utf8Name, utf8NameBufSize)
        ''' 			);
        ''' 		}
        ''' 
        ''' 

        ' Start accepting Unicode text input events, show keyboard 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_StartTextInput()
        End Sub


        ' Check if unicode input events are enabled 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_IsTextInputActive() As SDL_bool
        End Function


        ' Stop receiving any text input events, hide onscreen kbd 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_StopTextInput()
        End Sub


        ' Set the rectangle used for text input, hint for IME 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_SetTextInputRect(ByRef rect As SDL_Rect)
        End Sub


        ' Does the platform support an on-screen keyboard? 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HasScreenKeyboardSupport() As SDL_bool
        End Function


        ' Is the on-screen keyboard shown for a given window? 
        ' window is an SDL_Window pointer 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_IsScreenKeyboardShown(ByVal window As IntPtr) As SDL_bool
        End Function


#End Region

#Region "SDL_mouse.c"

        ' Note: SDL_Cursor is a typedef normally. We'll treat it as
        ' 		 * an IntPtr, because C# doesn't do typedefs. Yay!
        ' 		 

        ' System cursor types 
        Public Enum SDL_SystemCursor
            SDL_SYSTEM_CURSOR_ARROW    ' Arrow
            SDL_SYSTEM_CURSOR_IBEAM    ' I-beam
            SDL_SYSTEM_CURSOR_WAIT     ' Wait
            SDL_SYSTEM_CURSOR_CROSSHAIR    ' Crosshair
            SDL_SYSTEM_CURSOR_WAITARROW    ' Small wait cursor (or Wait if not available)
            SDL_SYSTEM_CURSOR_SIZENWSE ' Double arrow pointing northwest and southeast
            SDL_SYSTEM_CURSOR_SIZENESW ' Double arrow pointing northeast and southwest
            SDL_SYSTEM_CURSOR_SIZEWE   ' Double arrow pointing west and east
            SDL_SYSTEM_CURSOR_SIZENS   ' Double arrow pointing north and south
            SDL_SYSTEM_CURSOR_SIZEALL  ' Four pointed arrow pointing north, south, east, and west
            SDL_SYSTEM_CURSOR_NO       ' Slashed circle or crossbones
            SDL_SYSTEM_CURSOR_HAND     ' Hand
            SDL_NUM_SYSTEM_CURSORS
        End Enum


        ' Get the window which currently has mouse focus 
        ' Return value is an SDL_Window pointer 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetMouseFocus() As IntPtr
        End Function


        ' Get the current state of the mouse 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetMouseState(<Out> ByRef x As Integer, <Out> ByRef y As Integer) As UInteger
        End Function


        ' Get the current state of the mouse 
        ' This overload allows for passing NULL to x 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetMouseState(ByVal x As IntPtr, <Out> ByRef y As Integer) As UInteger
        End Function


        ' Get the current state of the mouse 
        ' This overload allows for passing NULL to y 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetMouseState(<Out> ByRef x As Integer, ByVal y As IntPtr) As UInteger
        End Function


        ' Get the current state of the mouse 
        ' This overload allows for passing NULL to both x and y 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetMouseState(ByVal x As IntPtr, ByVal y As IntPtr) As UInteger
        End Function


        ' Get the current state of the mouse, in relation to the desktop.
        ' 		 * Only available in 2.0.4 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetGlobalMouseState(<Out> ByRef x As Integer, <Out> ByRef y As Integer) As UInteger
        End Function


        ' Get the current state of the mouse, in relation to the desktop.
        ' 		 * Only available in 2.0.4 or higher.
        ' 		 * This overload allows for passing NULL to x.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetGlobalMouseState(ByVal x As IntPtr, <Out> ByRef y As Integer) As UInteger
        End Function


        ' Get the current state of the mouse, in relation to the desktop.
        ' 		 * Only available in 2.0.4 or higher.
        ' 		 * This overload allows for passing NULL to y.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetGlobalMouseState(<Out> ByRef x As Integer, ByVal y As IntPtr) As UInteger
        End Function


        ' Get the current state of the mouse, in relation to the desktop.
        ' 		 * Only available in 2.0.4 or higher.
        ' 		 * This overload allows for passing NULL to both x and y
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetGlobalMouseState(ByVal x As IntPtr, ByVal y As IntPtr) As UInteger
        End Function


        ' Get the mouse state with relative coords
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetRelativeMouseState(<Out> ByRef x As Integer, <Out> ByRef y As Integer) As UInteger
        End Function


        ' Set the mouse cursor's position (within a window) 
        ' window is an SDL_Window pointer 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_WarpMouseInWindow(ByVal window As IntPtr, ByVal x As Integer, ByVal y As Integer)
        End Sub


        ' Set the mouse cursor's position in global screen space.
        ' 		 * Only available in 2.0.4 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_WarpMouseGlobal(ByVal x As Integer, ByVal y As Integer) As Integer
        End Function


        ' Enable/Disable relative mouse mode (grabs mouse, rel coords) 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SetRelativeMouseMode(ByVal enabled As SDL_bool) As Integer
        End Function


        ' Capture the mouse, to track input outside an SDL window.
        ' 		 * Only available in 2.0.4 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_CaptureMouse(ByVal enabled As SDL_bool) As Integer
        End Function


        ' Query if the relative mouse mode is enabled 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetRelativeMouseMode() As SDL_bool
        End Function


        ' Create a cursor from bitmap data (amd mask) in MSB format.
        ' 		 * data and mask are byte arrays, and w must be a multiple of 8.
        ' 		 * return value is an SDL_Cursor pointer.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_CreateCursor(ByVal data As IntPtr, ByVal mask As IntPtr, ByVal w As Integer, ByVal h As Integer, ByVal hot_x As Integer, ByVal hot_y As Integer) As IntPtr
        End Function


        ' Create a cursor from an SDL_Surface.
        ' 		 * IntPtr refers to an SDL_Cursor*, surface to an SDL_Surface*
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_CreateColorCursor(ByVal surface As IntPtr, ByVal hot_x As Integer, ByVal hot_y As Integer) As IntPtr
        End Function


        ' Create a cursor from a system cursor id.
        ' 		 * return value is an SDL_Cursor pointer
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_CreateSystemCursor(ByVal id As SDL_SystemCursor) As IntPtr
        End Function


        ' Set the active cursor.
        ' 		 * cursor is an SDL_Cursor pointer
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_SetCursor(ByVal cursor As IntPtr)
        End Sub


        ' Return the active cursor
        ' 		 * return value is an SDL_Cursor pointer
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetCursor() As IntPtr
        End Function


        ' Frees a cursor created with one of the CreateCursor functions.
        ' 		 * cursor in an SDL_Cursor pointer
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_FreeCursor(ByVal cursor As IntPtr)
        End Sub


        ' Toggle whether or not the cursor is shown 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_ShowCursor(ByVal toggle As Integer) As Integer
        End Function

        Public Function SDL_BUTTON(ByVal X As UInteger) As UInteger
            ' If only there were a better way of doing this in C#
            Return 1 << CInt(X) - 1
        End Function

        Public Const SDL_BUTTON_LEFT As UInteger = 1
        Public Const SDL_BUTTON_MIDDLE As UInteger = 2
        Public Const SDL_BUTTON_RIGHT As UInteger = 3
        Public Const SDL_BUTTON_X1 As UInteger = 4
        Public Const SDL_BUTTON_X2 As UInteger = 5
        Public ReadOnly SDL_BUTTON_LMASK As UInteger = SDL_BUTTON(SDL_BUTTON_LEFT)
        Public ReadOnly SDL_BUTTON_MMASK As UInteger = SDL_BUTTON(SDL_BUTTON_MIDDLE)
        Public ReadOnly SDL_BUTTON_RMASK As UInteger = SDL_BUTTON(SDL_BUTTON_RIGHT)
        Public ReadOnly SDL_BUTTON_X1MASK As UInteger = SDL_BUTTON(SDL_BUTTON_X1)
        Public ReadOnly SDL_BUTTON_X2MASK As UInteger = SDL_BUTTON(SDL_BUTTON_X2)

#End Region

#Region "SDL_touch.h"

        Public Const SDL_TOUCH_MOUSEID As UInteger = UInteger.MaxValue

        Public Structure SDL_Finger
            Public id As Long ' SDL_FingerID
            Public x As Single
            Public y As Single
            Public pressure As Single
        End Structure


        ' Only available in 2.0.10 or higher. 
        Public Enum SDL_TouchDeviceType
            SDL_TOUCH_DEVICE_INVALID = -1
            SDL_TOUCH_DEVICE_DIRECT            ' touch screen with window-relative coordinates 
            SDL_TOUCH_DEVICE_INDIRECT_ABSOLUTE ' trackpad with absolute device coordinates 
            SDL_TOUCH_DEVICE_INDIRECT_RELATIVE  ' trackpad with screen cursor-relative coordinates 
        End Enum


        ' *
        ' 		 *  \brief Get the number of registered touch devices.
        ' 

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetNumTouchDevices() As Integer
        End Function


        ' *
        ' 		 *  \brief Get the touch ID with the given index, or 0 if the index is invalid.
        ' 

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetTouchDevice(ByVal index As Integer) As Long
        End Function


        ' *
        ' 		 *  \brief Get the number of active fingers for a given touch device.
        ' 

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetNumTouchFingers(ByVal touchID As Long) As Integer
        End Function


        ' *
        ' 		 *  \brief Get the finger object of the given touch, with the given index.
        ' 		 *  Returns pointer to SDL_Finger.
        ' 

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetTouchFinger(ByVal touchID As Long, ByVal index As Integer) As IntPtr
        End Function


        ' Only available in 2.0.10 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetTouchDeviceType(ByVal touchID As Long) As SDL_TouchDeviceType
        End Function


#End Region

#Region "SDL_joystick.h"

        Public Const SDL_HAT_CENTERED As Byte = &H0
        Public Const SDL_HAT_UP As Byte = &H1
        Public Const SDL_HAT_RIGHT As Byte = &H2
        Public Const SDL_HAT_DOWN As Byte = &H4
        Public Const SDL_HAT_LEFT As Byte = &H8
        Public Const SDL_HAT_RIGHTUP As Byte = SDL_HAT_RIGHT Or SDL_HAT_UP
        Public Const SDL_HAT_RIGHTDOWN As Byte = SDL_HAT_RIGHT Or SDL_HAT_DOWN
        Public Const SDL_HAT_LEFTUP As Byte = SDL_HAT_LEFT Or SDL_HAT_UP
        Public Const SDL_HAT_LEFTDOWN As Byte = SDL_HAT_LEFT Or SDL_HAT_DOWN

        Public Enum SDL_JoystickPowerLevel
            SDL_JOYSTICK_POWER_UNKNOWN = -1
            SDL_JOYSTICK_POWER_EMPTY
            SDL_JOYSTICK_POWER_LOW
            SDL_JOYSTICK_POWER_MEDIUM
            SDL_JOYSTICK_POWER_FULL
            SDL_JOYSTICK_POWER_WIRED
            SDL_JOYSTICK_POWER_MAX
        End Enum

        Public Enum SDL_JoystickType
            SDL_JOYSTICK_TYPE_UNKNOWN
            SDL_JOYSTICK_TYPE_GAMECONTROLLER
            SDL_JOYSTICK_TYPE_WHEEL
            SDL_JOYSTICK_TYPE_ARCADE_STICK
            SDL_JOYSTICK_TYPE_FLIGHT_STICK
            SDL_JOYSTICK_TYPE_DANCE_PAD
            SDL_JOYSTICK_TYPE_GUITAR
            SDL_JOYSTICK_TYPE_DRUM_KIT
            SDL_JOYSTICK_TYPE_ARCADE_PAD
        End Enum


        ' joystick refers to an SDL_Joystick*.
        ' 		 * Only available in 2.0.9 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickRumble(ByVal joystick As IntPtr, ByVal low_frequency_rumble As UShort, ByVal high_frequency_rumble As UShort, ByVal duration_ms As UInteger) As Integer
        End Function


        ' joystick refers to an SDL_Joystick* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_JoystickClose(ByVal joystick As IntPtr)
        End Sub

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickEventState(ByVal state As Integer) As Integer
        End Function


        ' joystick refers to an SDL_Joystick* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickGetAxis(ByVal joystick As IntPtr, ByVal axis As Integer) As Short
        End Function


        ' joystick refers to an SDL_Joystick*.
        ' 		 * Only available in 2.0.6 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickGetAxisInitialState(ByVal joystick As IntPtr, ByVal axis As Integer, <Out> ByRef state As UShort) As SDL_bool
        End Function


        ' joystick refers to an SDL_Joystick* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickGetBall(ByVal joystick As IntPtr, ByVal ball As Integer, <Out> ByRef dx As Integer, <Out> ByRef dy As Integer) As Integer
        End Function


        ' joystick refers to an SDL_Joystick* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickGetButton(ByVal joystick As IntPtr, ByVal button As Integer) As Byte
        End Function


        ' joystick refers to an SDL_Joystick* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickGetHat(ByVal joystick As IntPtr, ByVal hat As Integer) As Byte
        End Function


        ' joystick refers to an SDL_Joystick* 
        <DllImport(nativeLibName, EntryPoint:="SDL_JoystickName", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_JoystickName(ByVal joystick As IntPtr) As IntPtr
        End Function

        Public Function SDL_JoystickName(ByVal joystick As IntPtr) As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_JoystickName(joystick))
        End Function

        <DllImport(nativeLibName, EntryPoint:="SDL_JoystickNameForIndex", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_JoystickNameForIndex(ByVal device_index As Integer) As IntPtr
        End Function

        Public Function SDL_JoystickNameForIndex(ByVal device_index As Integer) As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_JoystickNameForIndex(device_index))
        End Function


        ' joystick refers to an SDL_Joystick* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickNumAxes(ByVal joystick As IntPtr) As Integer
        End Function


        ' joystick refers to an SDL_Joystick* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickNumBalls(ByVal joystick As IntPtr) As Integer
        End Function


        ' joystick refers to an SDL_Joystick* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickNumButtons(ByVal joystick As IntPtr) As Integer
        End Function


        ' joystick refers to an SDL_Joystick* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickNumHats(ByVal joystick As IntPtr) As Integer
        End Function


        ' IntPtr refers to an SDL_Joystick* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickOpen(ByVal device_index As Integer) As IntPtr
        End Function


        ' joystick refers to an SDL_Joystick* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_JoystickUpdate()
        End Sub


        ' joystick refers to an SDL_Joystick* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_NumJoysticks() As Integer
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickGetDeviceGUID(ByVal device_index As Integer) As Guid
        End Function


        ' joystick refers to an SDL_Joystick* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickGetGUID(ByVal joystick As IntPtr) As Guid
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_JoystickGetGUIDString(ByVal guid As Guid, ByVal pszGUID As Byte(), ByVal cbGUID As Integer)
        End Sub

        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_JoystickGetGUIDFromString", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe System.Guid INTERNAL_SDL_JoystickGetGUIDFromString(
        ''' 			byte* pchGUID
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe System.Guid SDL_JoystickGetGUIDFromString(string pchGuid)
        ''' 		{
        ''' 			int utf8PchGuidBufSize = SDL2.SDL.Utf8Size(pchGuid);
        ''' 			byte* utf8PchGuid = stackalloc byte[utf8PchGuidBufSize];
        ''' 			return SDL2.SDL.INTERNAL_SDL_JoystickGetGUIDFromString(
        ''' 				SDL2.SDL.Utf8Encode(pchGuid, utf8PchGuid, utf8PchGuidBufSize)
        ''' 			);
        ''' 		}
        ''' 
        ''' 

        ' Only available in 2.0.6 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickGetDeviceVendor(ByVal device_index As Integer) As UShort
        End Function


        ' Only available in 2.0.6 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickGetDeviceProduct(ByVal device_index As Integer) As UShort
        End Function


        ' Only available in 2.0.6 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickGetDeviceProductVersion(ByVal device_index As Integer) As UShort
        End Function


        ' Only available in 2.0.6 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickGetDeviceType(ByVal device_index As Integer) As SDL_JoystickType
        End Function


        ' int refers to an SDL_JoystickID.
        ' 		 * Only available in 2.0.6 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickGetDeviceInstanceID(ByVal device_index As Integer) As Integer
        End Function


        ' joystick refers to an SDL_Joystick*.
        ' 		 * Only available in 2.0.6 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickGetVendor(ByVal joystick As IntPtr) As UShort
        End Function


        ' joystick refers to an SDL_Joystick*.
        ' 		 * Only available in 2.0.6 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickGetProduct(ByVal joystick As IntPtr) As UShort
        End Function


        ' joystick refers to an SDL_Joystick*.
        ' 		 * Only available in 2.0.6 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickGetProductVersion(ByVal joystick As IntPtr) As UShort
        End Function


        ' joystick refers to an SDL_Joystick*.
        ' 		 * Only available in 2.0.6 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickGetType(ByVal joystick As IntPtr) As SDL_JoystickType
        End Function


        ' joystick refers to an SDL_Joystick* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickGetAttached(ByVal joystick As IntPtr) As SDL_bool
        End Function


        ' int refers to an SDL_JoystickID, joystick to an SDL_Joystick* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickInstanceID(ByVal joystick As IntPtr) As Integer
        End Function


        ' joystick refers to an SDL_Joystick*.
        ' 		 * Only available in 2.0.4 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickCurrentPowerLevel(ByVal joystick As IntPtr) As SDL_JoystickPowerLevel
        End Function


        ' int refers to an SDL_JoystickID, IntPtr to an SDL_Joystick*.
        ' 		 * Only available in 2.0.4 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickFromInstanceID(ByVal instance_id As Integer) As IntPtr
        End Function


        ' Only available in 2.0.7 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_LockJoysticks()
        End Sub


        ' Only available in 2.0.7 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_UnlockJoysticks()
        End Sub


        ' IntPtr refers to an SDL_Joystick*.
        ' 		 * Only available in 2.0.11 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickFromPlayerIndex(ByVal player_index As Integer) As IntPtr
        End Function


        ' IntPtr refers to an SDL_Joystick*.
        ' 		 * Only available in 2.0.11 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_JoystickSetPlayerIndex(ByVal joystick As IntPtr, ByVal player_index As Integer)
        End Sub


#End Region

#Region "SDL_gamecontroller.h"

        Public Enum SDL_GameControllerBindType
            SDL_CONTROLLER_BINDTYPE_NONE
            SDL_CONTROLLER_BINDTYPE_BUTTON
            SDL_CONTROLLER_BINDTYPE_AXIS
            SDL_CONTROLLER_BINDTYPE_HAT
        End Enum

        Public Enum SDL_GameControllerAxis
            SDL_CONTROLLER_AXIS_INVALID = -1
            SDL_CONTROLLER_AXIS_LEFTX
            SDL_CONTROLLER_AXIS_LEFTY
            SDL_CONTROLLER_AXIS_RIGHTX
            SDL_CONTROLLER_AXIS_RIGHTY
            SDL_CONTROLLER_AXIS_TRIGGERLEFT
            SDL_CONTROLLER_AXIS_TRIGGERRIGHT
            SDL_CONTROLLER_AXIS_MAX
        End Enum

        Public Enum SDL_GameControllerButton
            SDL_CONTROLLER_BUTTON_INVALID = -1
            SDL_CONTROLLER_BUTTON_A
            SDL_CONTROLLER_BUTTON_B
            SDL_CONTROLLER_BUTTON_X
            SDL_CONTROLLER_BUTTON_Y
            SDL_CONTROLLER_BUTTON_BACK
            SDL_CONTROLLER_BUTTON_GUIDE
            SDL_CONTROLLER_BUTTON_START
            SDL_CONTROLLER_BUTTON_LEFTSTICK
            SDL_CONTROLLER_BUTTON_RIGHTSTICK
            SDL_CONTROLLER_BUTTON_LEFTSHOULDER
            SDL_CONTROLLER_BUTTON_RIGHTSHOULDER
            SDL_CONTROLLER_BUTTON_DPAD_UP
            SDL_CONTROLLER_BUTTON_DPAD_DOWN
            SDL_CONTROLLER_BUTTON_DPAD_LEFT
            SDL_CONTROLLER_BUTTON_DPAD_RIGHT
            SDL_CONTROLLER_BUTTON_MAX
        End Enum

        Public Enum SDL_GameControllerType
            SDL_CONTROLLER_TYPE_UNKNOWN = 0
            SDL_CONTROLLER_TYPE_XBOX360
            SDL_CONTROLLER_TYPE_XBOXONE
            SDL_CONTROLLER_TYPE_PS3
            SDL_CONTROLLER_TYPE_PS4
            SDL_CONTROLLER_TYPE_NINTENDO_SWITCH_PRO
        End Enum


        ' FIXME: I'd rather this somehow be private...
        <StructLayout(LayoutKind.Sequential)>
        Public Structure INTERNAL_GameControllerButtonBind_hat
            Public hat As Integer
            Public hat_mask As Integer
        End Structure


        ' FIXME: I'd rather this somehow be private...
        <StructLayout(LayoutKind.Explicit)>
        Public Structure INTERNAL_GameControllerButtonBind_union
            <FieldOffset(0)>
            Public button As Integer
            <FieldOffset(0)>
            Public axis As Integer
            <FieldOffset(0)>
            Public hat As INTERNAL_GameControllerButtonBind_hat
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_GameControllerButtonBind
            Public bindType As SDL_GameControllerBindType
            Public value As INTERNAL_GameControllerButtonBind_union
        End Structure


        ' This exists to deal with C# being stupid about blittable types. 
        <StructLayout(LayoutKind.Sequential)>
        Private Structure INTERNAL_SDL_GameControllerButtonBind
            Public bindType As Integer
            ' Largest data type in the union is two ints in size 
            Public unionVal0 As Integer
            Public unionVal1 As Integer
        End Structure

        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_GameControllerAddMapping", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe int INTERNAL_SDL_GameControllerAddMapping(
        ''' 			byte* mappingString
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe int SDL_GameControllerAddMapping(
        ''' 			string mappingString
        ''' 		)
        ''' 		{
        ''' 			byte* utf8MappingString = SDL2.SDL.Utf8Encode(mappingString);
        ''' 			int result = SDL2.SDL.INTERNAL_SDL_GameControllerAddMapping(
        ''' 				utf8MappingString
        ''' 			);
        ''' 			System.Runtime.InteropServices.Marshal.FreeHGlobal((System.IntPtr)utf8MappingString);
        ''' 			return result;
        ''' 		}
        ''' 
        ''' 

        ' Only available in 2.0.6 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GameControllerNumMappings() As Integer
        End Function


        ' Only available in 2.0.6 or higher. 
        <DllImport(nativeLibName, EntryPoint:="SDL_GameControllerMappingForIndex", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GameControllerMappingForIndex(ByVal mapping_index As Integer) As IntPtr
        End Function

        Public Function SDL_GameControllerMappingForIndex(ByVal mapping_index As Integer) As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GameControllerMappingForIndex(mapping_index))
        End Function


        ' THIS IS AN RWops FUNCTION! 
        <DllImport(nativeLibName, EntryPoint:="SDL_GameControllerAddMappingsFromRW", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GameControllerAddMappingsFromRW(ByVal rw As IntPtr, ByVal freerw As Integer) As Integer
        End Function

        Public Function SDL_GameControllerAddMappingsFromFile(ByVal file As String) As Integer
            Dim rwops As IntPtr = SDL.SDL_RWFromFile(file, "rb")
            Return INTERNAL_SDL_GameControllerAddMappingsFromRW(rwops, 1)
        End Function

        <DllImport(nativeLibName, EntryPoint:="SDL_GameControllerMappingForGUID", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GameControllerMappingForGUID(ByVal guid As Guid) As IntPtr
        End Function

        Public Function SDL_GameControllerMappingForGUID(ByVal guid As Guid) As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GameControllerMappingForGUID(guid))
        End Function


        ' gamecontroller refers to an SDL_GameController* 
        <DllImport(nativeLibName, EntryPoint:="SDL_GameControllerMapping", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GameControllerMapping(ByVal gamecontroller As IntPtr) As IntPtr
        End Function

        Public Function SDL_GameControllerMapping(ByVal gamecontroller As IntPtr) As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GameControllerMapping(gamecontroller))
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_IsGameController(ByVal joystick_index As Integer) As SDL_bool
        End Function

        <DllImport(nativeLibName, EntryPoint:="SDL_GameControllerNameForIndex", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GameControllerNameForIndex(ByVal joystick_index As Integer) As IntPtr
        End Function

        Public Function SDL_GameControllerNameForIndex(ByVal joystick_index As Integer) As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GameControllerNameForIndex(joystick_index))
        End Function


        ' Only available in 2.0.9 or higher. 
        <DllImport(nativeLibName, EntryPoint:="SDL_GameControllerMappingForDeviceIndex", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GameControllerMappingForDeviceIndex(ByVal joystick_index As Integer) As IntPtr
        End Function

        Public Function SDL_GameControllerMappingForDeviceIndex(ByVal joystick_index As Integer) As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GameControllerMappingForDeviceIndex(joystick_index))
        End Function


        ' IntPtr refers to an SDL_GameController* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GameControllerOpen(ByVal joystick_index As Integer) As IntPtr
        End Function


        ' gamecontroller refers to an SDL_GameController* 
        <DllImport(nativeLibName, EntryPoint:="SDL_GameControllerName", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GameControllerName(ByVal gamecontroller As IntPtr) As IntPtr
        End Function

        Public Function SDL_GameControllerName(ByVal gamecontroller As IntPtr) As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GameControllerName(gamecontroller))
        End Function


        ' gamecontroller refers to an SDL_GameController*.
        ' 		 * Only available in 2.0.6 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GameControllerGetVendor(ByVal gamecontroller As IntPtr) As UShort
        End Function


        ' gamecontroller refers to an SDL_GameController*.
        ' 		 * Only available in 2.0.6 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GameControllerGetProduct(ByVal gamecontroller As IntPtr) As UShort
        End Function


        ' gamecontroller refers to an SDL_GameController*.
        ' 		 * Only available in 2.0.6 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GameControllerGetProductVersion(ByVal gamecontroller As IntPtr) As UShort
        End Function


        ' gamecontroller refers to an SDL_GameController* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GameControllerGetAttached(ByVal gamecontroller As IntPtr) As SDL_bool
        End Function


        ' IntPtr refers to an SDL_Joystick*
        ' 		 * gamecontroller refers to an SDL_GameController*
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GameControllerGetJoystick(ByVal gamecontroller As IntPtr) As IntPtr
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GameControllerEventState(ByVal state As Integer) As Integer
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_GameControllerUpdate()
        End Sub

        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_GameControllerGetAxisFromString", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe SDL2.SDL.SDL_GameControllerAxis INTERNAL_SDL_GameControllerGetAxisFromString(
        ''' 			byte* pchString
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe SDL2.SDL.SDL_GameControllerAxis SDL_GameControllerGetAxisFromString(
        ''' 			string pchString
        ''' 		)
        ''' 		{
        ''' 			int utf8PchStringBufSize = SDL2.SDL.Utf8Size(pchString);
        ''' 			byte* utf8PchString = stackalloc byte[utf8PchStringBufSize];
        ''' 			return SDL2.SDL.INTERNAL_SDL_GameControllerGetAxisFromString(
        ''' 				SDL2.SDL.Utf8Encode(pchString, utf8PchString, utf8PchStringBufSize)
        ''' 			);
        ''' 		}
        ''' 
        ''' 

        <DllImport(nativeLibName, EntryPoint:="SDL_GameControllerGetStringForAxis", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GameControllerGetStringForAxis(ByVal axis As SDL_GameControllerAxis) As IntPtr
        End Function

        Public Function SDL_GameControllerGetStringForAxis(ByVal axis As SDL_GameControllerAxis) As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GameControllerGetStringForAxis(axis))
        End Function


        ' gamecontroller refers to an SDL_GameController* 
        <DllImport(nativeLibName, EntryPoint:="SDL_GameControllerGetBindForAxis", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GameControllerGetBindForAxis(ByVal gamecontroller As IntPtr, ByVal axis As SDL_GameControllerAxis) As INTERNAL_SDL_GameControllerButtonBind
        End Function

        Public Function SDL_GameControllerGetBindForAxis(ByVal gamecontroller As IntPtr, ByVal axis As SDL_GameControllerAxis) As SDL_GameControllerButtonBind
            ' This is guaranteed to never be null
            Dim dumb As INTERNAL_SDL_GameControllerButtonBind = INTERNAL_SDL_GameControllerGetBindForAxis(gamecontroller, axis)
            Dim result As SDL_GameControllerButtonBind = New SDL_GameControllerButtonBind()
            result.bindType = CType(dumb.bindType, SDL_GameControllerBindType)
            result.value.hat.hat = dumb.unionVal0
            result.value.hat.hat_mask = dumb.unionVal1
            Return result
        End Function


        ' gamecontroller refers to an SDL_GameController* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GameControllerGetAxis(ByVal gamecontroller As IntPtr, ByVal axis As SDL_GameControllerAxis) As Short
        End Function

        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_GameControllerGetButtonFromString", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe SDL2.SDL.SDL_GameControllerButton INTERNAL_SDL_GameControllerGetButtonFromString(
        ''' 			byte* pchString
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe SDL2.SDL.SDL_GameControllerButton SDL_GameControllerGetButtonFromString(
        ''' 			string pchString
        ''' 		)
        ''' 		{
        ''' 			int utf8PchStringBufSize = SDL2.SDL.Utf8Size(pchString);
        ''' 			byte* utf8PchString = stackalloc byte[utf8PchStringBufSize];
        ''' 			return SDL2.SDL.INTERNAL_SDL_GameControllerGetButtonFromString(
        ''' 				SDL2.SDL.Utf8Encode(pchString, utf8PchString, utf8PchStringBufSize)
        ''' 			);
        ''' 		}
        ''' 
        ''' 

        <DllImport(nativeLibName, EntryPoint:="SDL_GameControllerGetStringForButton", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GameControllerGetStringForButton(ByVal button As SDL_GameControllerButton) As IntPtr
        End Function

        Public Function SDL_GameControllerGetStringForButton(ByVal button As SDL_GameControllerButton) As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GameControllerGetStringForButton(button))
        End Function


        ' gamecontroller refers to an SDL_GameController* 
        <DllImport(nativeLibName, EntryPoint:="SDL_GameControllerGetBindForButton", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GameControllerGetBindForButton(ByVal gamecontroller As IntPtr, ByVal button As SDL_GameControllerButton) As INTERNAL_SDL_GameControllerButtonBind
        End Function

        Public Function SDL_GameControllerGetBindForButton(ByVal gamecontroller As IntPtr, ByVal button As SDL_GameControllerButton) As SDL_GameControllerButtonBind
            ' This is guaranteed to never be null
            Dim dumb As INTERNAL_SDL_GameControllerButtonBind = INTERNAL_SDL_GameControllerGetBindForButton(gamecontroller, button)
            Dim result As SDL_GameControllerButtonBind = New SDL_GameControllerButtonBind()
            result.bindType = CType(dumb.bindType, SDL_GameControllerBindType)
            result.value.hat.hat = dumb.unionVal0
            result.value.hat.hat_mask = dumb.unionVal1
            Return result
        End Function


        ' gamecontroller refers to an SDL_GameController* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GameControllerGetButton(ByVal gamecontroller As IntPtr, ByVal button As SDL_GameControllerButton) As Byte
        End Function


        ' gamecontroller refers to an SDL_GameController*.
        ' 		 * Only available in 2.0.9 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GameControllerRumble(ByVal gamecontroller As IntPtr, ByVal low_frequency_rumble As UShort, ByVal high_frequency_rumble As UShort, ByVal duration_ms As UInteger) As Integer
        End Function


        ' gamecontroller refers to an SDL_GameController* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_GameControllerClose(ByVal gamecontroller As IntPtr)
        End Sub


        ' int refers to an SDL_JoystickID, IntPtr to an SDL_GameController*.
        ' 		 * Only available in 2.0.4 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GameControllerFromInstanceID(ByVal joyid As Integer) As IntPtr
        End Function


        ' Only available in 2.0.11 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GameControllerTypeForIndex(ByVal joystick_index As Integer) As SDL_GameControllerType
        End Function


        ' IntPtr refers to an SDL_GameController*.
        ' 		 * Only available in 2.0.11 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GameControllerGetType(ByVal gamecontroller As IntPtr) As SDL_GameControllerType
        End Function


        ' IntPtr refers to an SDL_GameController*.
        ' 		 * Only available in 2.0.11 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GameControllerFromPlayerIndex(ByVal player_index As Integer) As IntPtr
        End Function


        ' IntPtr refers to an SDL_GameController*.
        ' 		 * Only available in 2.0.11 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_GameControllerSetPlayerIndex(ByVal gamecontroller As IntPtr, ByVal player_index As Integer)
        End Sub


#End Region

#Region "SDL_haptic.h"

        ' SDL_HapticEffect type 
        Public Const SDL_HAPTIC_CONSTANT As UShort = 1 << 0
        Public Const SDL_HAPTIC_SINE As UShort = 1 << 1
        Public Const SDL_HAPTIC_LEFTRIGHT As UShort = 1 << 2
        Public Const SDL_HAPTIC_TRIANGLE As UShort = 1 << 3
        Public Const SDL_HAPTIC_SAWTOOTHUP As UShort = 1 << 4
        Public Const SDL_HAPTIC_SAWTOOTHDOWN As UShort = 1 << 5
        Public Const SDL_HAPTIC_SPRING As UShort = 1 << 7
        Public Const SDL_HAPTIC_DAMPER As UShort = 1 << 8
        Public Const SDL_HAPTIC_INERTIA As UShort = 1 << 9
        Public Const SDL_HAPTIC_FRICTION As UShort = 1 << 10
        Public Const SDL_HAPTIC_CUSTOM As UShort = 1 << 11
        Public Const SDL_HAPTIC_GAIN As UShort = 1 << 12
        Public Const SDL_HAPTIC_AUTOCENTER As UShort = 1 << 13
        Public Const SDL_HAPTIC_STATUS As UShort = 1 << 14
        Public Const SDL_HAPTIC_PAUSE As UShort = 1 << 15

        ' SDL_HapticDirection type 
        Public Const SDL_HAPTIC_POLAR As Byte = 0
        Public Const SDL_HAPTIC_CARTESIAN As Byte = 1
        Public Const SDL_HAPTIC_SPHERICAL As Byte = 2

        ' SDL_HapticRunEffect 
        Public Const SDL_HAPTIC_INFINITY As UInteger = 4294967295UI
        ''' Cannot convert StructDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitStructDeclaration(StructDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		[System.Runtime.InteropServices.@StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        ''' 		public unsafe struct SDL_HapticDirection
        ''' 		{
        ''' 			public byte type;
        ''' 			public fixed int dir[3];
        ''' 		}
        ''' 
        ''' 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_HapticDirection
            Public type As Byte
            <VBFixedArray(4)> Public dir() As Integer
        End Structure


        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_HapticConstant
            ' Header
            Public type As UShort
            Public direction As SDL.SDL_HapticDirection
            ' Replay
            Public length As UInteger
            Public delay As UShort
            ' Trigger
            Public button As UShort
            Public interval As UShort
            ' Constant
            Public level As Short
            ' Envelope
            Public attack_length As UShort
            Public attack_level As UShort
            Public fade_length As UShort
            Public fade_level As UShort
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_HapticPeriodic
            ' Header
            Public type As UShort
            Public direction As SDL.SDL_HapticDirection
            ' Replay
            Public length As UInteger
            Public delay As UShort
            ' Trigger
            Public button As UShort
            Public interval As UShort
            ' Periodic
            Public period As UShort
            Public magnitude As Short
            Public offset As Short
            Public phase As UShort
            ' Envelope
            Public attack_length As UShort
            Public attack_level As UShort
            Public fade_length As UShort
            Public fade_level As UShort
            ' Header
            ' Replay
            ' Trigger
            ' Condition
        End Structure

        ''' Cannot convert StructDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitStructDeclaration(StructDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		[System.Runtime.InteropServices.@StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        ''' 		public unsafe struct SDL_HapticCondition
        ''' 		{
        ''' 			// Header
        ''' 			public ushort type;
        ''' 			public SDL2.SDL.SDL_HapticDirection direction;
        ''' 			// Replay
        ''' 			public uint length;
        ''' 			public ushort delay;
        ''' 			// Trigger
        ''' 			public ushort button;
        ''' 			public ushort interval;
        ''' 			// Condition
        ''' 			public fixed ushort right_sat[3];
        ''' 			public fixed ushort left_sat[3];
        ''' 			public fixed short right_coeff[3];
        ''' 			public fixed short left_coeff[3];
        ''' 			public fixed ushort deadband[3];
        ''' 			public fixed short center[3];
        ''' 		}
        ''' 
        ''' 
        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_HapticCondition
            Public type As UShort
            Public direction As SDL.SDL_HapticDirection
            Public length As UInteger
            Public delay As UInteger
            Public button As UShort
            Public interval As UShort
            <VBFixedArray(3)> Public right_sat() As UShort
            <VBFixedArray(3)> Public left_sat() As UShort
            <VBFixedArray(3)> Public right_coeff() As Short
            <VBFixedArray(3)> Public left_coeff() As Short
            <VBFixedArray(3)> Public deadband() As UShort
            <VBFixedArray(3)> Public center() As Short
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_HapticRamp
            ' Header
            Public type As UShort
            Public direction As SDL.SDL_HapticDirection
            ' Replay
            Public length As UInteger
            Public delay As UShort
            ' Trigger
            Public button As UShort
            Public interval As UShort
            ' Ramp
            Public start As Short
            Public [end] As Short
            ' Envelope
            Public attack_length As UShort
            Public attack_level As UShort
            Public fade_length As UShort
            Public fade_level As UShort
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_HapticLeftRight
            ' Header
            Public type As UShort
            ' Replay
            Public length As UInteger
            ' Rumble
            Public large_magnitude As UShort
            Public small_magnitude As UShort
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_HapticCustom
            ' Header
            Public type As UShort
            Public direction As SDL.SDL_HapticDirection
            ' Replay
            Public length As UInteger
            Public delay As UShort
            ' Trigger
            Public button As UShort
            Public interval As UShort
            ' Custom
            Public channels As Byte
            Public period As UShort
            Public samples As UShort
            Public data As IntPtr ' Uint16*
            ' Envelope
            Public attack_length As UShort
            Public attack_level As UShort
            Public fade_length As UShort
            Public fade_level As UShort
        End Structure

        <StructLayout(LayoutKind.Explicit)>
        Public Structure SDL_HapticEffect
            <FieldOffset(0)>
            Public type As UShort
            <FieldOffset(0)>
            Public constant As SDL_HapticConstant
            <FieldOffset(0)>
            Public periodic As SDL_HapticPeriodic
            <FieldOffset(0)>
            Public condition As SDL.SDL_HapticCondition
            <FieldOffset(0)>
            Public ramp As SDL_HapticRamp
            <FieldOffset(0)>
            Public leftright As SDL_HapticLeftRight
            <FieldOffset(0)>
            Public custom As SDL_HapticCustom
        End Structure


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_HapticClose(ByVal haptic As IntPtr)
        End Sub


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_HapticDestroyEffect(ByVal haptic As IntPtr, ByVal effect As Integer)
        End Sub


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticEffectSupported(ByVal haptic As IntPtr, ByRef effect As SDL_HapticEffect) As Integer
        End Function


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticGetEffectStatus(ByVal haptic As IntPtr, ByVal effect As Integer) As Integer
        End Function


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticIndex(ByVal haptic As IntPtr) As Integer
        End Function


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, EntryPoint:="SDL_HapticName", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_HapticName(ByVal device_index As Integer) As IntPtr
        End Function

        Public Function SDL_HapticName(ByVal device_index As Integer) As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_HapticName(device_index))
        End Function


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticNewEffect(ByVal haptic As IntPtr, ByRef effect As SDL_HapticEffect) As Integer
        End Function


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticNumAxes(ByVal haptic As IntPtr) As Integer
        End Function


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticNumEffects(ByVal haptic As IntPtr) As Integer
        End Function


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticNumEffectsPlaying(ByVal haptic As IntPtr) As Integer
        End Function


        ' IntPtr refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticOpen(ByVal device_index As Integer) As IntPtr
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticOpened(ByVal device_index As Integer) As Integer
        End Function


        ' IntPtr refers to an SDL_Haptic*, joystick to an SDL_Joystick* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticOpenFromJoystick(ByVal joystick As IntPtr) As IntPtr
        End Function


        ' IntPtr refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticOpenFromMouse() As IntPtr
        End Function


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticPause(ByVal haptic As IntPtr) As Integer
        End Function


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticQuery(ByVal haptic As IntPtr) As UInteger
        End Function


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticRumbleInit(ByVal haptic As IntPtr) As Integer
        End Function


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticRumblePlay(ByVal haptic As IntPtr, ByVal strength As Single, ByVal length As UInteger) As Integer
        End Function


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticRumbleStop(ByVal haptic As IntPtr) As Integer
        End Function


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticRumbleSupported(ByVal haptic As IntPtr) As Integer
        End Function


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticRunEffect(ByVal haptic As IntPtr, ByVal effect As Integer, ByVal iterations As UInteger) As Integer
        End Function


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticSetAutocenter(ByVal haptic As IntPtr, ByVal autocenter As Integer) As Integer
        End Function


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticSetGain(ByVal haptic As IntPtr, ByVal gain As Integer) As Integer
        End Function


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticStopAll(ByVal haptic As IntPtr) As Integer
        End Function


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticStopEffect(ByVal haptic As IntPtr, ByVal effect As Integer) As Integer
        End Function


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticUnpause(ByVal haptic As IntPtr) As Integer
        End Function


        ' haptic refers to an SDL_Haptic* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HapticUpdateEffect(ByVal haptic As IntPtr, ByVal effect As Integer, ByRef data As SDL_HapticEffect) As Integer
        End Function


        ' joystick refers to an SDL_Joystick* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_JoystickIsHaptic(ByVal joystick As IntPtr) As Integer
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_MouseIsHaptic() As Integer
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_NumHaptics() As Integer
        End Function


#End Region

#Region "SDL_sensor.h"

        ' This region is only available in 2.0.9 or higher. 

        Public Enum SDL_SensorType
            SDL_SENSOR_INVALID = -1
            SDL_SENSOR_UNKNOWN
            SDL_SENSOR_ACCEL
            SDL_SENSOR_GYRO
        End Enum

        Public Const SDL_STANDARD_GRAVITY As Single = 9.80665F

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_NumSensors() As Integer
        End Function

        <DllImport(nativeLibName, EntryPoint:="SDL_SensorGetDeviceName", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_SensorGetDeviceName(ByVal device_index As Integer) As IntPtr
        End Function

        Public Function SDL_SensorGetDeviceName(ByVal device_index As Integer) As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_SensorGetDeviceName(device_index))
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SensorGetDeviceType(ByVal device_index As Integer) As SDL_SensorType
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SensorGetDeviceNonPortableType(ByVal device_index As Integer) As Integer
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SensorGetDeviceInstanceID(ByVal device_index As Integer) As Integer
        End Function


        ' IntPtr refers to an SDL_Sensor* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SensorOpen(ByVal device_index As Integer) As IntPtr
        End Function


        ' IntPtr refers to an SDL_Sensor* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SensorFromInstanceID(ByVal instance_id As Integer) As IntPtr
        End Function


        ' sensor refers to an SDL_Sensor* 
        <DllImport(nativeLibName, EntryPoint:="SDL_SensorGetName", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_SensorGetName(ByVal sensor As IntPtr) As IntPtr
        End Function

        Public Function SDL_SensorGetName(ByVal sensor As IntPtr) As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_SensorGetName(sensor))
        End Function


        ' sensor refers to an SDL_Sensor* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SensorGetType(ByVal sensor As IntPtr) As SDL_SensorType
        End Function


        ' sensor refers to an SDL_Sensor* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SensorGetNonPortableType(ByVal sensor As IntPtr) As Integer
        End Function


        ' sensor refers to an SDL_Sensor* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SensorGetInstanceID(ByVal sensor As IntPtr) As Integer
        End Function


        ' sensor refers to an SDL_Sensor* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SensorGetData(ByVal sensor As IntPtr, ByVal data As Single(), ByVal num_values As Integer) As Integer
        End Function


        ' sensor refers to an SDL_Sensor* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_SensorClose(ByVal sensor As IntPtr)
        End Sub

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_SensorUpdate()
        End Sub


#End Region

#Region "SDL_audio.h"

        Public Const SDL_AUDIO_MASK_BITSIZE As UShort = &HFF
        Public Const SDL_AUDIO_MASK_DATATYPE As UShort = 1 << 8
        Public Const SDL_AUDIO_MASK_ENDIAN As UShort = 1 << 12
        Public Const SDL_AUDIO_MASK_SIGNED As UShort = 1 << 15

        Public Function SDL_AUDIO_BITSIZE(ByVal x As UShort) As UShort
            Return x And SDL_AUDIO_MASK_BITSIZE
        End Function

        Public Function SDL_AUDIO_ISFLOAT(ByVal x As UShort) As Boolean
            Return (x And SDL_AUDIO_MASK_DATATYPE) <> 0
        End Function

        Public Function SDL_AUDIO_ISBIGENDIAN(ByVal x As UShort) As Boolean
            Return (x And SDL_AUDIO_MASK_ENDIAN) <> 0
        End Function

        Public Function SDL_AUDIO_ISSIGNED(ByVal x As UShort) As Boolean
            Return (x And SDL_AUDIO_MASK_SIGNED) <> 0
        End Function

        Public Function SDL_AUDIO_ISINT(ByVal x As UShort) As Boolean
            Return (x And SDL_AUDIO_MASK_DATATYPE) = 0
        End Function

        Public Function SDL_AUDIO_ISLITTLEENDIAN(ByVal x As UShort) As Boolean
            Return (x And SDL_AUDIO_MASK_ENDIAN) = 0
        End Function

        Public Function SDL_AUDIO_ISUNSIGNED(ByVal x As UShort) As Boolean
            Return (x And SDL_AUDIO_MASK_SIGNED) = 0
        End Function

        Public Const AUDIO_U8 As UShort = &H8
        Public Const AUDIO_S8 As UShort = &H8008
        Public Const AUDIO_U16LSB As UShort = &H10
        Public Const AUDIO_S16LSB As UShort = &H8010
        Public Const AUDIO_U16MSB As UShort = &H1010
        Public Const AUDIO_S16MSB As UShort = &H9010
        Public Const AUDIO_U16 As UShort = AUDIO_U16LSB
        Public Const AUDIO_S16 As UShort = AUDIO_S16LSB
        Public Const AUDIO_S32LSB As UShort = &H8020
        Public Const AUDIO_S32MSB As UShort = &H9020
        Public Const AUDIO_S32 As UShort = AUDIO_S32LSB
        Public Const AUDIO_F32LSB As UShort = &H8120
        Public Const AUDIO_F32MSB As UShort = &H9120
        Public Const AUDIO_F32 As UShort = AUDIO_F32LSB
        Public ReadOnly AUDIO_U16SYS As UShort = If(BitConverter.IsLittleEndian, AUDIO_U16LSB, AUDIO_U16MSB)
        Public ReadOnly AUDIO_S16SYS As UShort = If(BitConverter.IsLittleEndian, AUDIO_S16LSB, AUDIO_S16MSB)
        Public ReadOnly AUDIO_S32SYS As UShort = If(BitConverter.IsLittleEndian, AUDIO_S32LSB, AUDIO_S32MSB)
        Public ReadOnly AUDIO_F32SYS As UShort = If(BitConverter.IsLittleEndian, AUDIO_F32LSB, AUDIO_F32MSB)
        Public Const SDL_AUDIO_ALLOW_FREQUENCY_CHANGE As UInteger = &H1
        Public Const SDL_AUDIO_ALLOW_FORMAT_CHANGE As UInteger = &H2
        Public Const SDL_AUDIO_ALLOW_CHANNELS_CHANGE As UInteger = &H4
        Public Const SDL_AUDIO_ALLOW_SAMPLES_CHANGE As UInteger = &H8
        Public Const SDL_AUDIO_ALLOW_ANY_CHANGE As UInteger = SDL_AUDIO_ALLOW_FREQUENCY_CHANGE Or SDL_AUDIO_ALLOW_FORMAT_CHANGE Or SDL_AUDIO_ALLOW_CHANNELS_CHANGE Or SDL_AUDIO_ALLOW_SAMPLES_CHANGE
        Public Const SDL_MIX_MAXVOLUME As Integer = 128

        Public Enum SDL_AudioStatus
            SDL_AUDIO_STOPPED
            SDL_AUDIO_PLAYING
            SDL_AUDIO_PAUSED
        End Enum

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_AudioSpec
            Public freq As Integer
            Public format As UShort ' SDL_AudioFormat
            Public channels As Byte
            Public silence As Byte
            Public samples As UShort
            Public size As UInteger
            Public callback As SDL_AudioCallback
            Public userdata As IntPtr ' void*
        End Structure


        ' userdata refers to a void*, stream to a Uint8 
        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub SDL_AudioCallback(ByVal userdata As IntPtr, ByVal stream As IntPtr, ByVal len As Integer)
        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_AudioInit", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe int INTERNAL_SDL_AudioInit(
        ''' 			byte* driver_name
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe int SDL_AudioInit(string driver_name)
        ''' 		{
        ''' 			int utf8DriverNameBufSize = SDL2.SDL.Utf8Size(driver_name);
        ''' 			byte* utf8DriverName = stackalloc byte[utf8DriverNameBufSize];
        ''' 			return SDL2.SDL.INTERNAL_SDL_AudioInit(
        ''' 				SDL2.SDL.Utf8Encode(driver_name, utf8DriverName, utf8DriverNameBufSize)
        ''' 			);
        ''' 		}
        ''' 
        ''' 

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_AudioQuit()
        End Sub

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_CloseAudio()
        End Sub


        ' dev refers to an SDL_AudioDeviceID 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_CloseAudioDevice(ByVal dev As UInteger)
        End Sub


        ' audio_buf refers to a malloc()'d buffer from SDL_LoadWAV 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_FreeWAV(ByVal audio_buf As IntPtr)
        End Sub

        <DllImport(nativeLibName, EntryPoint:="SDL_GetAudioDeviceName", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GetAudioDeviceName(ByVal index As Integer, ByVal iscapture As Integer) As IntPtr
        End Function

        Public Function SDL_GetAudioDeviceName(ByVal index As Integer, ByVal iscapture As Integer) As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GetAudioDeviceName(index, iscapture))
        End Function


        ' dev refers to an SDL_AudioDeviceID 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetAudioDeviceStatus(ByVal dev As UInteger) As SDL_AudioStatus
        End Function

        <DllImport(nativeLibName, EntryPoint:="SDL_GetAudioDriver", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GetAudioDriver(ByVal index As Integer) As IntPtr
        End Function

        Public Function SDL_GetAudioDriver(ByVal index As Integer) As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GetAudioDriver(index))
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetAudioStatus() As SDL_AudioStatus
        End Function

        <DllImport(nativeLibName, EntryPoint:="SDL_GetCurrentAudioDriver", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GetCurrentAudioDriver() As IntPtr
        End Function

        Public Function SDL_GetCurrentAudioDriver() As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GetCurrentAudioDriver())
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetNumAudioDevices(ByVal iscapture As Integer) As Integer
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetNumAudioDrivers() As Integer
        End Function


        ' audio_buf will refer to a malloc()'d byte buffer 
        ' THIS IS AN RWops FUNCTION! 
        <DllImport(nativeLibName, EntryPoint:="SDL_LoadWAV_RW", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_LoadWAV_RW(ByVal src As IntPtr, ByVal freesrc As Integer, ByRef spec As SDL_AudioSpec, <Out> ByRef audio_buf As IntPtr, <Out> ByRef audio_len As UInteger) As IntPtr
        End Function

        Public Function SDL_LoadWAV(ByVal file As String, ByRef spec As SDL_AudioSpec, <Out> ByRef audio_buf As IntPtr, <Out> ByRef audio_len As UInteger) As SDL_AudioSpec
            Dim result As SDL_AudioSpec
            Dim rwops As IntPtr = SDL.SDL_RWFromFile(file, "rb")
            Dim result_ptr As IntPtr = INTERNAL_SDL_LoadWAV_RW(rwops, 1, spec, audio_buf, audio_len)
            result = CType(Marshal.PtrToStructure(result_ptr, GetType(SDL_AudioSpec)), SDL_AudioSpec)
            Return result
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_LockAudio()
        End Sub


        ' dev refers to an SDL_AudioDeviceID 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_LockAudioDevice(ByVal dev As UInteger)
        End Sub

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_MixAudio(
        <Out()>
        <MarshalAs(UnmanagedType.LPArray, ArraySubType:=UnmanagedType.U1, SizeParamIndex:=2)> ByVal dst As Byte(),
        <[In]()>
        <MarshalAs(UnmanagedType.LPArray, ArraySubType:=UnmanagedType.U1, SizeParamIndex:=2)> ByVal src As Byte(), ByVal len As UInteger, ByVal volume As Integer)
        End Sub


        ' format refers to an SDL_AudioFormat 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_MixAudioFormat(
        <Out()>
        <MarshalAs(UnmanagedType.LPArray, ArraySubType:=UnmanagedType.U1, SizeParamIndex:=3)> ByVal dst As Byte(),
        <[In]()>
        <MarshalAs(UnmanagedType.LPArray, ArraySubType:=UnmanagedType.U1, SizeParamIndex:=3)> ByVal src As Byte(), ByVal format As UShort, ByVal len As UInteger, ByVal volume As Integer)
        End Sub

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_OpenAudio(ByRef desired As SDL_AudioSpec, <Out> ByRef obtained As SDL_AudioSpec) As Integer
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_OpenAudio(ByRef desired As SDL_AudioSpec, ByVal obtained As IntPtr) As Integer
        End Function

        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* uint refers to an SDL_AudioDeviceID */
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_OpenAudioDevice", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe uint INTERNAL_SDL_OpenAudioDevice(
        ''' 			byte* device,
        ''' 			int iscapture,
        ''' 			ref SDL2.SDL.SDL_AudioSpec desired,
        ''' 			out SDL2.SDL.SDL_AudioSpec obtained,
        ''' 			int allowed_changes
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe uint SDL_OpenAudioDevice(
        ''' 			string device,
        ''' 			int iscapture,
        ''' 			ref SDL2.SDL.SDL_AudioSpec desired,
        ''' 			out SDL2.SDL.SDL_AudioSpec obtained,
        ''' 			int allowed_changes
        ''' 		)
        ''' 		{
        ''' 			int utf8DeviceBufSize = SDL2.SDL.Utf8Size(device);
        ''' 			byte* utf8Device = stackalloc byte[utf8DeviceBufSize];
        ''' 			return SDL2.SDL.INTERNAL_SDL_OpenAudioDevice(
        ''' 				SDL2.SDL.Utf8Encode(device, utf8Device, utf8DeviceBufSize),
        ''' 				iscapture,
        ''' 				ref desired,
        ''' 				out obtained,
        ''' 				allowed_changes
        ''' 			);
        ''' 		}
        ''' 
        ''' 

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_PauseAudio(ByVal pause_on As Integer)
        End Sub


        ' dev refers to an SDL_AudioDeviceID 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_PauseAudioDevice(ByVal dev As UInteger, ByVal pause_on As Integer)
        End Sub

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_UnlockAudio()
        End Sub


        ' dev refers to an SDL_AudioDeviceID 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_UnlockAudioDevice(ByVal dev As UInteger)
        End Sub


        ' dev refers to an SDL_AudioDeviceID, data to a void*
        ' 		 * Only available in 2.0.4 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_QueueAudio(ByVal dev As UInteger, ByVal data As IntPtr, ByVal len As UInteger) As Integer
        End Function


        ' dev refers to an SDL_AudioDeviceID, data to a void*
        ' 		 * Only available in 2.0.5 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_DequeueAudio(ByVal dev As UInteger, ByVal data As IntPtr, ByVal len As UInteger) As UInteger
        End Function


        ' dev refers to an SDL_AudioDeviceID
        ' 		 * Only available in 2.0.4 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetQueuedAudioSize(ByVal dev As UInteger) As UInteger
        End Function


        ' dev refers to an SDL_AudioDeviceID
        ' 		 * Only available in 2.0.4 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_ClearQueuedAudio(ByVal dev As UInteger)
        End Sub


        ' src_format and dst_format refer to SDL_AudioFormats.
        ' 		 * IntPtr refers to an SDL_AudioStream*.
        ' 		 * Only available in 2.0.7 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_NewAudioStream(ByVal src_format As UShort, ByVal src_channels As Byte, ByVal src_rate As Integer, ByVal dst_format As UShort, ByVal dst_channels As Byte, ByVal dst_rate As Integer) As IntPtr
        End Function


        ' stream refers to an SDL_AudioStream*, buf to a void*.
        ' 		 * Only available in 2.0.7 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_AudioStreamPut(ByVal stream As IntPtr, ByVal buf As IntPtr, ByVal len As Integer) As Integer
        End Function


        ' stream refers to an SDL_AudioStream*, buf to a void*.
        ' 		 * Only available in 2.0.7 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_AudioStreamGet(ByVal stream As IntPtr, ByVal buf As IntPtr, ByVal len As Integer) As Integer
        End Function


        ' stream refers to an SDL_AudioStream*.
        ' 		 * Only available in 2.0.7 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_AudioStreamAvailable(ByVal stream As IntPtr) As Integer
        End Function


        ' stream refers to an SDL_AudioStream*.
        ' 		 * Only available in 2.0.7 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_AudioStreamClear(ByVal stream As IntPtr)
        End Sub


        ' stream refers to an SDL_AudioStream*.
        ' 		 * Only available in 2.0.7 or higher.
        ' 		 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_FreeAudioStream(ByVal stream As IntPtr)
        End Sub


#End Region

#Region "SDL_timer.h"

        ' System timers rely on different OS mechanisms depending on
        ' 		 * which operating system SDL2 is compiled against.
        ' 		 

        ' Compare tick values, return true if A has passed B. Introduced in SDL 2.0.1,
        ' 		 * but does not require it (it was a macro).
        ' 		 
        Public Function SDL_TICKS_PASSED(ByVal A As UInteger, ByVal B As UInteger) As Boolean
            Return CInt(B - A) <= 0
        End Function


        ' Delays the thread's processing based on the milliseconds parameter 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_Delay(ByVal ms As UInteger)
        End Sub


        ' Returns the milliseconds that have passed since SDL was initialized 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetTicks() As UInteger
        End Function


        ' Get the current value of the high resolution counter 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetPerformanceCounter() As ULong
        End Function


        ' Get the count per second of the high resolution counter 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetPerformanceFrequency() As ULong
        End Function


        ' param refers to a void* 
        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function SDL_TimerCallback(ByVal interval As UInteger, ByVal param As IntPtr) As UInteger


        ' int refers to an SDL_TimerID, param to a void* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_AddTimer(ByVal interval As UInteger, ByVal callback As SDL_TimerCallback, ByVal param As IntPtr) As Integer
        End Function


        ' id refers to an SDL_TimerID 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_RemoveTimer(ByVal id As Integer) As SDL_bool
        End Function


#End Region

#Region "SDL_system.h"

        ' Windows 

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Function SDL_WindowsMessageHook(ByVal userdata As IntPtr, ByVal hWnd As IntPtr, ByVal message As UInteger, ByVal wParam As ULong, ByVal lParam As Long) As IntPtr

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_SetWindowsMessageHook(ByVal callback As SDL_WindowsMessageHook, ByVal userdata As IntPtr)
        End Sub


        ' iOS 

        <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
        Public Delegate Sub SDL_iPhoneAnimationCallback(ByVal p As IntPtr)

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_iPhoneSetAnimationCallback(ByVal window As IntPtr, ByVal interval As Integer, ByVal callback As SDL_iPhoneAnimationCallback, ByVal callbackParam As IntPtr) As Integer ' SDL_Window* 
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_iPhoneSetEventPump(ByVal enabled As SDL_bool)
        End Sub


        ' Android 

        Public Const SDL_ANDROID_EXTERNAL_STORAGE_READ As Integer = &H1
        Public Const SDL_ANDROID_EXTERNAL_STORAGE_WRITE As Integer = &H2


        ' IntPtr refers to a JNIEnv* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_AndroidGetJNIEnv() As IntPtr
        End Function


        ' IntPtr refers to a jobject 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_AndroidGetActivity() As IntPtr
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_IsAndroidTV() As SDL_bool
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_IsChromebook() As SDL_bool
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_IsDeXMode() As SDL_bool
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_AndroidBackButton()
        End Sub

        <DllImport(nativeLibName, EntryPoint:="SDL_AndroidGetInternalStoragePath", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_AndroidGetInternalStoragePath() As IntPtr
        End Function

        Public Function SDL_AndroidGetInternalStoragePath() As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_AndroidGetInternalStoragePath())
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_AndroidGetExternalStorageState() As Integer
        End Function

        <DllImport(nativeLibName, EntryPoint:="SDL_AndroidGetExternalStorageState", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_AndroidGetExternalStoragePath() As IntPtr
        End Function

        Public Function SDL_AndroidGetExternalStoragePath() As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_AndroidGetExternalStoragePath())
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetAndroidSDKVersion() As Integer
        End Function


        ' WinRT 

        Public Enum SDL_WinRT_DeviceFamily
            SDL_WINRT_DEVICEFAMILY_UNKNOWN
            SDL_WINRT_DEVICEFAMILY_DESKTOP
            SDL_WINRT_DEVICEFAMILY_MOBILE
            SDL_WINRT_DEVICEFAMILY_XBOX
        End Enum

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_WinRTGetDeviceFamily() As SDL_WinRT_DeviceFamily
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_IsTablet() As SDL_bool
        End Function


#End Region

#Region "SDL_syswm.h"

        Public Enum SDL_SYSWM_TYPE
            SDL_SYSWM_UNKNOWN
            SDL_SYSWM_WINDOWS
            SDL_SYSWM_X11
            SDL_SYSWM_DIRECTFB
            SDL_SYSWM_COCOA
            SDL_SYSWM_UIKIT
            SDL_SYSWM_WAYLAND
            SDL_SYSWM_MIR
            SDL_SYSWM_WINRT
            SDL_SYSWM_ANDROID
            SDL_SYSWM_VIVANTE
            SDL_SYSWM_OS2
            SDL_SYSWM_HAIKU
        End Enum


        ' FIXME: I wish these weren't public...
        <StructLayout(LayoutKind.Sequential)>
        Public Structure INTERNAL_windows_wminfo
            Public window As IntPtr ' Refers to an HWND
            Public hdc As IntPtr ' Refers to an HDC
            Public hinstance As IntPtr ' Refers to an HINSTANCE
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure INTERNAL_winrt_wminfo
            Public window As IntPtr ' Refers to an IInspectable*
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure INTERNAL_x11_wminfo
            Public display As IntPtr ' Refers to a Display*
            Public window As IntPtr ' Refers to a Window (XID, use ToInt64!)
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure INTERNAL_directfb_wminfo
            Public dfb As IntPtr ' Refers to an IDirectFB*
            Public window As IntPtr ' Refers to an IDirectFBWindow*
            Public surface As IntPtr ' Refers to an IDirectFBSurface*
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure INTERNAL_cocoa_wminfo
            Public window As IntPtr ' Refers to an NSWindow*
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure INTERNAL_uikit_wminfo
            Public window As IntPtr ' Refers to a UIWindow*
            Public framebuffer As UInteger
            Public colorbuffer As UInteger
            Public resolveFramebuffer As UInteger
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure INTERNAL_wayland_wminfo
            Public display As IntPtr ' Refers to a wl_display*
            Public surface As IntPtr ' Refers to a wl_surface*
            Public shell_surface As IntPtr ' Refers to a wl_shell_surface*
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure INTERNAL_mir_wminfo
            Public connection As IntPtr ' Refers to a MirConnection*
            Public surface As IntPtr ' Refers to a MirSurface*
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure INTERNAL_android_wminfo
            Public window As IntPtr ' Refers to an ANativeWindow
            Public surface As IntPtr ' Refers to an EGLSurface
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure INTERNAL_vivante_wminfo
            Public display As IntPtr ' Refers to an EGLNativeDisplayType
            Public window As IntPtr ' Refers to an EGLNativeWindowType
        End Structure

        <StructLayout(LayoutKind.Explicit)>
        Public Structure INTERNAL_SysWMDriverUnion
            <FieldOffset(0)>
            Public win As INTERNAL_windows_wminfo
            <FieldOffset(0)>
            Public winrt As INTERNAL_winrt_wminfo
            <FieldOffset(0)>
            Public x11 As INTERNAL_x11_wminfo
            <FieldOffset(0)>
            Public dfb As INTERNAL_directfb_wminfo
            <FieldOffset(0)>
            Public cocoa As INTERNAL_cocoa_wminfo
            <FieldOffset(0)>
            Public uikit As INTERNAL_uikit_wminfo
            <FieldOffset(0)>
            Public wl As INTERNAL_wayland_wminfo
            <FieldOffset(0)>
            Public mir As INTERNAL_mir_wminfo
            <FieldOffset(0)>
            Public android As INTERNAL_android_wminfo
            <FieldOffset(0)>
            Public vivante As INTERNAL_vivante_wminfo
            ' private int dummy;
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SDL_SysWMinfo
            Public version As n_SDL_version
            Public subsystem As SDL_SYSWM_TYPE
            Public info As INTERNAL_SysWMDriverUnion
        End Structure


        ' window refers to an SDL_Window* 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetWindowWMInfo(ByVal window As IntPtr, ByRef info As SDL_SysWMinfo) As SDL_bool
        End Function


#End Region

#Region "SDL_filesystem.h"

        ' Only available in 2.0.1 or higher. 
        <DllImport(nativeLibName, EntryPoint:="SDL_GetBasePath", CallingConvention:=CallingConvention.Cdecl)>
        Private Function INTERNAL_SDL_GetBasePath() As IntPtr
        End Function

        Public Function SDL_GetBasePath() As String
            Return SDL.UTF8_ToManaged(INTERNAL_SDL_GetBasePath(), True)

            ' Only available in 2.0.1 or higher. 
        End Function

        ''' Cannot convert MethodDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterListSyntax'.
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 
        ''' 		/* Only available in 2.0.1 or higher. */
        ''' 		[System.Runtime.InteropServices.@DllImportAttribute(SDL2.SDL.nativeLibName, EntryPoint = "SDL_GetPrefPath", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        ''' 		private static extern unsafe System.IntPtr INTERNAL_SDL_GetPrefPath(
        ''' 			byte* org,
        ''' 			byte* app
        ''' 		);
        ''' 
        ''' 
        ''' Cannot convert MethodDeclarationSyntax, System.NotSupportedException: UnsafeKeyword is not supported!
        '''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass29_0.<ConvertModifiersCore>b__3(SyntaxToken x)
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
        '''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
        '''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiers(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor)
        '''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitMethodDeclaration(MethodDeclarationSyntax node)
        '''    at Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax.Accept[TResult](CSharpSyntaxVisitor`1 visitor)
        '''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
        '''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
        ''' 
        ''' Input:
        ''' 		public static unsafe string SDL_GetPrefPath(string org, string app)
        ''' 		{
        ''' 			int utf8OrgBufSize = SDL2.SDL.Utf8SizeNullable(org);
        ''' 			byte* utf8Org = stackalloc byte[utf8OrgBufSize];
        ''' 
        ''' 			int utf8AppBufSize = SDL2.SDL.Utf8SizeNullable(app);
        ''' 			byte* utf8App = stackalloc byte[utf8AppBufSize];
        ''' 
        ''' 			return SDL2.SDL.UTF8_ToManaged(
        ''' 				SDL2.SDL.INTERNAL_SDL_GetPrefPath(
        ''' 					SDL2.SDL.Utf8EncodeNullable(org, utf8Org, utf8OrgBufSize),
        ''' 					SDL2.SDL.Utf8EncodeNullable(app, utf8App, utf8AppBufSize)
        ''' 				),
        ''' 				true
        ''' 			);
        ''' 		}
        ''' 
        ''' 

#End Region

#Region "SDL_power.h"

        Public Enum SDL_PowerState
            SDL_POWERSTATE_UNKNOWN = 0
            SDL_POWERSTATE_ON_BATTERY
            SDL_POWERSTATE_NO_BATTERY
            SDL_POWERSTATE_CHARGING
            SDL_POWERSTATE_CHARGED
        End Enum

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetPowerInfo(<Out> ByRef secs As Integer, <Out> ByRef pct As Integer) As SDL_PowerState
        End Function


#End Region

#Region "SDL_cpuinfo.h"

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetCPUCount() As Integer
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetCPUCacheLineSize() As Integer
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HasRDTSC() As SDL_bool
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HasAltiVec() As SDL_bool
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HasMMX() As SDL_bool
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_Has3DNow() As SDL_bool
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HasSSE() As SDL_bool
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HasSSE2() As SDL_bool
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HasSSE3() As SDL_bool
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HasSSE41() As SDL_bool
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HasSSE42() As SDL_bool
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HasAVX() As SDL_bool
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HasAVX2() As SDL_bool
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HasAVX512F() As SDL_bool
        End Function

        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_HasNEON() As SDL_bool
        End Function


        ' Only available in 2.0.1 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_GetSystemRAM() As Integer
        End Function


        ' Only available in SDL 2.0.10 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SIMDGetAlignment() As UInteger
        End Function


        ' Only available in SDL 2.0.10 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Function SDL_SIMDAlloc(ByVal len As UInteger) As IntPtr
        End Function


        ' Only available in SDL 2.0.10 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_SIMDFree(ByVal ptr As IntPtr)
        End Sub


        ' Only available in SDL 2.0.11 or higher. 
        <DllImport(nativeLibName, CallingConvention:=CallingConvention.Cdecl)>
        Public Sub SDL_HasARMSIMD()
        End Sub

#End Region
    End Module
End Namespace
